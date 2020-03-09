using AdminMensajeria.Models;
using AdminMensajeria.Models.WebAPI;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;

namespace AdminMensajeria.Controllers
{

    public class WebApiController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();
        private readonly RestClient _client;
        private readonly string _url = "https://sigomx.carvajaltys.mx/";

        public WebApiController()
        {
            _client = new RestClient(_url);
        }


        // GET: WebApi
        public ActionResult Index()
        {
           
            return View();

        }

        // GET: WebApi
        public ActionResult ConsumeAPI(DateTime desde)
        {
            LoginModel login = new LoginModel() { usuario = "etiquetas_rest", password = "untCLv05@*" };

            var token = GetToken(login);

            var lineas = GetAll(token, 0, desde).ToList();

            return PartialView("~/Views/WebApi/ConsumeAPI.cshtml",lineas);

        }



        public string GetToken(LoginModel login)
        {
            try
            {
                var request = new RestRequest("api/JWTController/Login", Method.POST) { RequestFormat = DataFormat.Json };
                request.AddBody(login);
                var response = _client.Execute<Object>(request);
                response.Data = JsonConvert.DeserializeObject<String>(response.Content);

                return response.Data.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<EnvioModel> GetAll(string token,  int statusConnection, DateTime desde)
        {
            var parameter = "?FechaIni=" + desde.ToString("yyyy-MM-dd HH:mm:ss");
            var request = new RestRequest("api/ValuesController/getReporteEtiquetas" + parameter, Method.GET) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "Bearer " + token);
            var response = _client.Execute<List<EnvioModel>>(request);
            statusConnection = (int)response.StatusCode;
            response.Data = JsonConvert.DeserializeObject<List<EnvioModel>>(response.Content);

            return response.Data;
        }


        public JsonResult CrateMasivo(string Referencia, decimal NumEtiquetas, string Destinatario, string Direccion, string TipoEnvio, int Fila)
        {
            Resultados resultado = new Resultados();
            resultado.Fila = Fila;

            var existe = db.OPE_SOLICITUD.Where(x => x.Folio == Referencia).ToList();

            if (existe.Count() > 0)
            {
                resultado.Result = true;
                resultado.Valor = existe.FirstOrDefault().IdSolicitud;
                resultado.Preregistrada = true;

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

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
            Solicitud.Folio = Referencia;
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
                Productos.Descripcion = "Etiquetas";
                Productos.Cantidad = NumEtiquetas;
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
                        Punto.DescripcionPuntoEntRec = Destinatario;
                        Punto.DirCompletaPuntoEntRec = Direccion;
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

    }
}
