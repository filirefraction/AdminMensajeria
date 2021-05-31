using AdminMensajeria.Clases;
using AdminMensajeria.Models;
using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

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
            data = ((IEnumerable<int>)data).OrderBy<int, int>((Func<int, int>)(c => c)).ToArray<int>();

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
            Resultados resultados = new Resultados();
            if (upload != null && upload.ContentLength > 0)
            {
                Stream inputStream = upload.InputStream;
                IExcelDataReader self;
                if (upload.FileName.EndsWith(".xls"))
                    self = ExcelReaderFactory.CreateBinaryReader(inputStream);
                else if (upload.FileName.EndsWith(".xlsx"))
                {
                    self = ExcelReaderFactory.CreateOpenXmlReader(inputStream);
                }
                else
                {
                    resultados.Result = false;
                    resultados.Mensaje = "Este formato de archivo no es compatible";
                    return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
                }
                int fieldCount = self.FieldCount;
                int rowCount = self.RowCount;
                DataTable table1 = new DataTable();
                DataTable dataTable = new DataTable();
                try
                {
                    DataTable table2 = self.AsDataSet().Tables[0];
                    table1.Columns.Add("Destinatario (*,200)");
                    table1.Columns.Add("Referencia 1 (*,200)");
                    table1.Columns.Add("Cant (*)");
                    table1.Columns.Add("Destino (*,100)");
                    table1.Columns.Add("Calle (*,100)");
                    table1.Columns.Add("#Ext (*,20)");
                    table1.Columns.Add("#Int (100)");
                    table1.Columns.Add("Referencia 2 (200)");
                    table1.Columns.Add("Colonia (*,100)");
                    table1.Columns.Add("C.P. (*,20)");
                    table1.Columns.Add("Municipio (*,100)");
                    table1.Columns.Add("Estado (*,100)");
                    table1.Columns.Add("País (*,100)");
                    table1.Columns.Add("Valido");
                    table1.Columns.Add("#");
                    int num = 0;
                    for (int index = 1; index < table2.Rows.Count; ++index)
                    {
                        DataRow row = table1.NewRow();
                        for (int columnIndex = 0; columnIndex < table2.Columns.Count; ++columnIndex)
                        {
                            row[columnIndex] = (object)table2.Rows[index][columnIndex].ToString();
                            ++num;
                        }
                        table1.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    resultados.Result = false;
                    resultados.Mensaje = "Imposible Subir Archivo!";
                    return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
                }
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(table1);
                self.Close();
                self.Dispose();
                this.Session["tmpdata"] = (object)dataSet.Tables[0];
                resultados.Result = true;
                return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
            }
            resultados.Result = false;
            resultados.Mensaje = "No Eligio Ningun Archivo";
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CrateMasivo(
       string Nombre,
       string Producto,
       Decimal Cantidad,
       string Destinatario,
       string Calle,
       string NoExt,
       string NoInt,
       string Referencia,
       string Colonia,
       string CodigoPostal,
       string Municipio,
       string Estado,
       string Pais,
       int Fila)
        {
            Resultados resultados = new Resultados();
            resultados.Fila = Fila;
            if (this.Session["IdUsuario"] == null)
            {
                resultados.Redirecciona = true;
                return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
            }
            int num = (int)this.Session["IdUsuario"];
            OPE_SOLICITUD Solicitud = new OPE_SOLICITUD();
            OPE_SOLICITUDPRODUCTO entity1 = new OPE_SOLICITUDPRODUCTO();
            OPE_SOLICITUDPUNTOSENTREC entity2 = new OPE_SOLICITUDPUNTOSENTREC();
            Solicitud.IdUsuario = num;
            Solicitud.FechaCreacion = helper.dateTimeZone(DateTime.Now);
            Solicitud.RequiereAcuse = false;
            Solicitud.Emergencia = false;
            Solicitud.EstatusSolicitud = 1;
            try
            {
                this.db.OPE_SOLICITUD.Add(Solicitud);
                this.db.SaveChanges();
                resultados.Result = true;
                resultados.Valor = this.db.OPE_SOLICITUD.OrderByDescending<OPE_SOLICITUD, int>((Expression<Func<OPE_SOLICITUD, int>>)(a => a.IdSolicitud)).Select<OPE_SOLICITUD, int>((Expression<Func<OPE_SOLICITUD, int>>)(a => a.IdSolicitud)).First<int>();
            }
            catch (Exception ex)
            {
                resultados.Result = false;
                resultados.Mensaje = "No se pudo Regsitrar Solicitud: " + ex.Message;
                resultados.Valor = 0;
            }
            if (resultados.Result)
            {
                entity1.IdSolicitud = resultados.Valor;
                entity1.Descripcion = Producto;
                entity1.Cantidad = new Decimal?(Cantidad);
                entity1.Recibido = false;
                try
                {
                    this.db.OPE_SOLICITUDPRODUCTO.Add(entity1);
                    this.db.SaveChanges();
                    resultados.Result = true;
                }
                catch (Exception ex)
                {
                    resultados.Result = false;
                    resultados.Mensaje = "No se pudo Registrar Producto: " + ex.Message;
                }
                if (resultados.Result)
                {
                    entity2.IdSolicitud = resultados.Valor;
                    entity2.IdPuntoEntRec = new int?(this.db.GEN_USUARIO.Where<GEN_USUARIO>((Expression<Func<GEN_USUARIO, bool>>)(p => p.IdUsuario == Solicitud.IdUsuario)).Select<GEN_USUARIO, int>((Expression<Func<GEN_USUARIO, int>>)(p => p.IdPuntoEntRec)).FirstOrDefault<int>());
                    entity2.TipoPunto = 1;
                    entity2.Problema = false;
                    entity2.EstatusPuntosEntRec = false;
                    try
                    {
                        this.db.OPE_SOLICITUDPUNTOSENTREC.Add(entity2);
                        this.db.SaveChanges();
                        resultados.Result = true;
                    }
                    catch (Exception ex)
                    {
                        resultados.Result = false;
                        resultados.Mensaje = "No se pudo Registrar Punto Remitente: " + ex.Message;
                    }
                    if (resultados.Result)
                    {
                        entity2.IdSolicitud = resultados.Valor;
                        entity2.IdPuntoEntRec = new int?();
                        entity2.TipoPunto = 2;
                        entity2.DestinatarioPuntoEntRec = Nombre;
                        entity2.DescripcionPuntoEntRec = Destinatario;
                        entity2.CallePuntosEntRec = Calle;
                        entity2.NoExtPuntosEntRec = NoExt;
                        entity2.NoIntPuntosEntRec = NoInt;
                        entity2.ReferenciaPuntosEntRec = Referencia;
                        entity2.ColoniaPuntosEntRec = Colonia;
                        entity2.CodigoPostalPuntosEntRec = CodigoPostal;
                        entity2.MunicipioPuntosEntRec = Municipio;
                        entity2.EstadoPuntosEntRec = Estado;
                        entity2.PaisPuntosEntRec = Pais;
                        entity2.Problema = false;
                        entity2.EstatusPuntosEntRec = false;
                        try
                        {
                            this.db.OPE_SOLICITUDPUNTOSENTREC.Add(entity2);
                            this.db.SaveChanges();
                            resultados.Result = true;
                        }
                        catch (Exception ex)
                        {
                            resultados.Result = false;
                            resultados.Mensaje = "No se pudo Registrar Punto Entrega: " + ex.Message;
                        }
                    }
                }
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LimpiarTabla()
        {
            Session["tmpdata"] = null;  //store datatable into session
            return Json(JsonRequestBehavior.AllowGet);
        }

    }
}