using AdminMensajeria.Models;
using ExcelDataReader;
using System;
using System.Net;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;
using RestSharp;


namespace AdminMensajeria.Controllers
{
 

    public class CargaMasivaController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: CargaMasiva
        public ActionResult Index()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = (DataTable)Session["tmpdata"];
            }
            catch (Exception ex)
            {

            }

            return View(dt);
        }

        public ActionResult ImpresionMasiva()
        {
            ArrayList numeros = new ArrayList();

            numeros = (ArrayList)Session["Numeros"];
            ViewBag.Numeros = numeros;

            return View(numeros);
        }

        public ActionResult Formato(int? id)
        {

            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            return PartialView(oPE_SOLICITUD);
        }



        public JsonResult Impresion(int[] data)
        {
            data = data.OrderBy(c => c).ToArray();
            ArrayList idList = new ArrayList();

            foreach (var item in data)
            {
                idList.Add(item);
            }


            Session["Numeros"] = idList;

            return Json(JsonRequestBehavior.AllowGet);
        }









        public JsonResult CargarTabla(HttpPostedFileWrapper upload)
        {

            Resultados Resultado = new Resultados();



            if (upload != null && upload.ContentLength > 0)
            {
                // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                // to get started. This is how we avoid dependencies on ACE or Interop:
                Stream stream = upload.InputStream;

                IExcelDataReader reader = null;


                if (upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    Resultado.Result = false;
                    Resultado.Mensaje = "Este formato de archivo no es compatible";
                    return Json(Resultado, JsonRequestBehavior.AllowGet);
                }
                int fieldcount = reader.FieldCount;
                int rowcount = reader.RowCount;
                DataTable dt = new DataTable();
                DataRow row;
                DataTable dt_ = new DataTable();
                try
                {
                    dt_ = reader.AsDataSet().Tables[0];
                    //for (int i = 0; i < dt_.Columns.Count; i++)
                    //{
                    //    dt.Columns.Add(dt_.Rows[0][i].ToString());
                    //}
                    dt.Columns.Add("Destinatario (*,200)");
                    dt.Columns.Add("Referencia 1 (*,200)");
                    dt.Columns.Add("Cant (*)");
                    dt.Columns.Add("Destino (*,100)");
                    dt.Columns.Add("Calle (*,100)");
                    dt.Columns.Add("#Ext (*,20)");
                    dt.Columns.Add("#Int (100)");
                    dt.Columns.Add("Referencia 2 (200)");
                    dt.Columns.Add("Colonia (*,100)");
                    dt.Columns.Add("C.P. (*,20)");
                    dt.Columns.Add("Municipio (*,100)");
                    dt.Columns.Add("Estado (*,100)");
                    dt.Columns.Add("País (*,100)");
                    dt.Columns.Add("Valido");
                    dt.Columns.Add("#");

                    int rowcounter = 0;
                    for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                    {
                        row = dt.NewRow();

                        for (int col = 0; col < dt_.Columns.Count; col++)
                        {
                            row[col] = dt_.Rows[row_][col].ToString();
                            rowcounter++;
                        }
                        dt.Rows.Add(row);
                    }

                }
                catch (Exception ex)
                {

                    Resultado.Result = false;
                    Resultado.Mensaje = "Imposible Subir Archivo!";
                    return Json(Resultado, JsonRequestBehavior.AllowGet);
                }

                DataSet result = new DataSet();
                result.Tables.Add(dt);
                reader.Close();
                reader.Dispose();
                DataTable tmp = result.Tables[0];
                Session["tmpdata"] = tmp;  //store datatable into session

                Resultado.Result = true;
                return Json(Resultado, JsonRequestBehavior.AllowGet);

            }
            else
            {
                Resultado.Result = false;
                Resultado.Mensaje = "No Eligio Ningun Archivo";
                return Json(Resultado, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult CrateMasivo(string Nombre, string Producto, decimal Cantidad, string Destinatario, string Calle, string NoExt, string NoInt, string Referencia, string Colonia, string CodigoPostal, string Municipio, string Estado, string Pais, int Fila)
        {
            Resultados resultado = new Resultados();
            resultado.Fila = Fila;

            int IdUsuario = 0;
            if (Session["IdUsuario"] == null)
            {
                resultado.Redirecciona = true;
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            else
                IdUsuario = (int)Session["IdUsuario"];


            OPE_SOLICITUD Solicitud = new OPE_SOLICITUD();
            OPE_SOLICITUDPRODUCTO Productos = new OPE_SOLICITUDPRODUCTO();
            OPE_SOLICITUDPUNTOSENTREC Punto = new OPE_SOLICITUDPUNTOSENTREC();


            //Primero Reguistra Solicitud
            Solicitud.IdUsuario = IdUsuario; // Aqui va Variable de sesión
            Solicitud.FechaCreacion = DateTime.Now; //Local
            //Solicitud.FechaCreacion = DateTime.Now.AddHours(1); //Producción
            Solicitud.RequiereAcuse = false;
            Solicitud.Emergencia = false;
            Solicitud.EstatusSolicitud = 1;
            try
            {
                db.OPE_SOLICITUD.Add(Solicitud);
                db.SaveChanges();
                resultado.Result = true;
                resultado.Valor = (from a in db.OPE_SOLICITUD orderby a.IdSolicitud descending select a.IdSolicitud).First();

                //Crea Imagen QR
                var writer = new BarcodeWriter() // Si un barcodeWriter para generar un codigo QR (O.O)
                {
                    Format = BarcodeFormat.QR_CODE, //setearle el tipo de codigo que generara.
                    Options = new EncodingOptions()
                    {
                        Height = 100,
                        Width = 100,
                        Margin = 1, // el margen que tendra el codigo con el restro de la imagen
                    },
                };

                // Generar el codigo, este metodo retorna una bitmap
                Bitmap bitmap = writer.Write(resultado.Valor.ToString());

                // guardar el bitmap con el formato deseado y la locacion deseada
                bitmap.Save(String.Format(Server.MapPath("~/Image/qr/") + "{0}.png", resultado.Valor.ToString()), ImageFormat.Png);


            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = "No se pudo Regsitrar Solicitud: " + ex.Message;
                resultado.Valor = 0;
            }

            if (resultado.Result == true)
            {
                //Registra Producto
                Productos.IdSolicitud = resultado.Valor;
                Productos.Descripcion = Producto;
                Productos.Cantidad = Cantidad;
                Productos.Recibido = false;
                try
                {
                    db.OPE_SOLICITUDPRODUCTO.Add(Productos);
                    db.SaveChanges();
                    resultado.Result = true;
                }
                catch (Exception ex)
                {
                    resultado.Result = false;
                    resultado.Mensaje = "No se pudo Registrar Producto: " + ex.Message;
                }

                if (resultado.Result == true)
                {
                    //RegistraPunto de Remitente
                    Punto.IdSolicitud = resultado.Valor;
                    Punto.IdPuntoEntRec = (from p in db.GEN_USUARIO where p.IdUsuario == Solicitud.IdUsuario select p.IdPuntoEntRec).FirstOrDefault();
                    Punto.TipoPunto = 1;
                    Punto.Problema = false;
                    Punto.EstatusPuntosEntRec = false;

                    try
                    {
                        db.OPE_SOLICITUDPUNTOSENTREC.Add(Punto);
                        db.SaveChanges();
                        resultado.Result = true;
                    }
                    catch (Exception ex)
                    {
                        resultado.Result = false;
                        resultado.Mensaje = "No se pudo Registrar Punto Remitente: " + ex.Message;
                    }

                    if (resultado.Result == true)
                    {
                        //RegistraPunto de Entrega
                        Punto.IdSolicitud = resultado.Valor;
                        Punto.IdPuntoEntRec = null;
                        Punto.TipoPunto = 2;
                        Punto.DestinatarioPuntoEntRec = Nombre;
                        Punto.DescripcionPuntoEntRec = Destinatario;
                        Punto.CallePuntosEntRec = Calle;
                        Punto.NoExtPuntosEntRec = NoExt;
                        Punto.NoIntPuntosEntRec = NoInt;
                        Punto.ReferenciaPuntosEntRec = Referencia;
                        Punto.ColoniaPuntosEntRec = Colonia;
                        Punto.CodigoPostalPuntosEntRec = CodigoPostal;
                        Punto.MunicipioPuntosEntRec = Municipio;
                        Punto.EstadoPuntosEntRec = Estado;
                        Punto.PaisPuntosEntRec = Pais;
                        Punto.Problema = false;
                        Punto.EstatusPuntosEntRec = false;
                        try
                        {
                            db.OPE_SOLICITUDPUNTOSENTREC.Add(Punto);
                            db.SaveChanges();
                            resultado.Result = true;
                        }
                        catch (Exception ex)
                        {
                            resultado.Result = false;
                            resultado.Mensaje = "No se pudo Registrar Punto Entrega: " + ex.Message;
                        }

                    }


                }


            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LimpiarTabla()
        {
            Session["tmpdata"] = null;  //store datatable into session
            return Json(JsonRequestBehavior.AllowGet);
        }

    }
}