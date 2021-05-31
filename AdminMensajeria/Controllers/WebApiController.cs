using AdminMensajeria.Clases;
using AdminMensajeria.Models;
using AdminMensajeria.Models.WebAPI;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace AdminMensajeria.Controllers
{
    //public class WebApiController : Controller
    //{
    //    private MensajeriaEntities db = new MensajeriaEntities();
    //    private readonly RestClient _client;
    //    private readonly string _url = "https://sigomx.carvajaltys.mx/";

    //    public WebApiController() => this._client = new RestClient(this._url);

    //    public ActionResult Index() => (ActionResult)this.View();

    //    public ActionResult ConsumeAPI(DateTime desde) => (ActionResult)this.PartialView("~/Views/WebApi/ConsumeAPI.cshtml", (object)this.GetAll(this.GetToken(new LoginModel()
    //    {
    //        usuario = "etiquetas_rest",
    //        password = "untCLv05@*"
    //    }), 0, desde).ToList<EnvioModel>());

    //    public string GetToken(LoginModel login)
    //    {
    //        try
    //        {
    //            RestRequest restRequest = new RestRequest("api/JWTController/Login", Method.POST)
    //            {
    //                RequestFormat = DataFormat.Json
    //            };
    //            restRequest.AddJsonBody((object)login);
    //            IRestResponse<object> restResponse = this._client.Execute<object>((IRestRequest)restRequest);
    //            restResponse.Data = (object)JsonConvert.DeserializeObject<string>(restResponse.Content);
    //            return restResponse.Data.ToString();
    //        }
    //        catch (Exception)
    //        {
    //            return (string)null;
    //        }
    //    }

    //    public IEnumerable<EnvioModel> GetAll(
    //      string token,
    //      int statusConnection,
    //      DateTime desde)
    //    {
    //        RestRequest restRequest = new RestRequest("api/ValuesController/getReporteEtiquetas" + ("?FechaIni=" + desde.ToString("yyyy-MM-dd HH:mm:ss")))
    //        {
    //            RequestFormat = DataFormat.Json
    //        };
    //        restRequest.AddHeader("Authorization", "Bearer " + token);
    //        IRestResponse<List<EnvioModel>> restResponse = this._client.Execute<List<EnvioModel>>((IRestRequest)restRequest);
    //        statusConnection = (int)restResponse.StatusCode;
    //        restResponse.Data = JsonConvert.DeserializeObject<List<EnvioModel>>(restResponse.Content);
    //        return (IEnumerable<EnvioModel>)restResponse.Data;
    //    }

    //    public JsonResult CrateMasivo(
    //      string Referencia,
    //      Decimal NumEtiquetas,
    //      string Destinatario,
    //      string Direccion,
    //      string TipoEnvio,
    //      int Fila)
    //    {
    //        Resultados resultados = new Resultados();
    //        resultados.Fila = Fila;
    //        List<OPE_SOLICITUD> list = this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.Folio == Referencia)).ToList<OPE_SOLICITUD>();
    //        if (list.Count<OPE_SOLICITUD>() > 0)
    //        {
    //            resultados.Result = true;
    //            resultados.Valor = list.FirstOrDefault<OPE_SOLICITUD>().IdSolicitud;
    //            resultados.Preregistrada = true;
    //            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
    //        }
    //        if (this.Session["IdUsuario"] == null)
    //        {
    //            resultados.Redirecciona = true;
    //            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
    //        }
    //        int num = (int)this.Session["IdUsuario"];
    //        OPE_SOLICITUD Solicitud = new OPE_SOLICITUD();
    //        OPE_SOLICITUDPRODUCTO entity1 = new OPE_SOLICITUDPRODUCTO();
    //        OPE_SOLICITUDPUNTOSENTREC entity2 = new OPE_SOLICITUDPUNTOSENTREC();
    //        Solicitud.IdUsuario = num;
    //        Solicitud.FechaCreacion = helper.dateTimeZone(DateTime.Now);
    //        Solicitud.Folio = Referencia;
    //        Solicitud.RequiereAcuse = false;
    //        Solicitud.Emergencia = false;
    //        Solicitud.EstatusSolicitud = 1;
    //        try
    //        {
    //            this.db.OPE_SOLICITUD.Add(Solicitud);
    //            this.db.SaveChanges();
    //            resultados.Result = true;
    //            resultados.Valor = this.db.OPE_SOLICITUD.OrderByDescending<OPE_SOLICITUD, int>((Expression<Func<OPE_SOLICITUD, int>>)(a => a.IdSolicitud)).Select<OPE_SOLICITUD, int>((Expression<Func<OPE_SOLICITUD, int>>)(a => a.IdSolicitud)).First<int>();
    //        }
    //        catch (Exception ex)
    //        {
    //            resultados.Result = false;
    //            resultados.Mensaje = "No se pudo Regsitrar Solicitud: " + ex.Message;
    //            resultados.Valor = 0;
    //        }
    //        if (resultados.Result)
    //        {
    //            entity1.IdSolicitud = resultados.Valor;
    //            entity1.Descripcion = "Etiquetas";
    //            entity1.Cantidad = new Decimal?(NumEtiquetas);
    //            entity1.Recibido = false;
    //            try
    //            {
    //                this.db.OPE_SOLICITUDPRODUCTO.Add(entity1);
    //                this.db.SaveChanges();
    //                resultados.Result = true;
    //            }
    //            catch (Exception ex)
    //            {
    //                resultados.Result = false;
    //                resultados.Mensaje = "No se pudo Registrar Producto: " + ex.Message;
    //            }
    //            if (resultados.Result)
    //            {
    //                entity2.IdSolicitud = resultados.Valor;
    //                entity2.IdPuntoEntRec = new int?(this.db.GEN_USUARIO.Where<GEN_USUARIO>((Expression<Func<GEN_USUARIO, bool>>)(p => p.IdUsuario == Solicitud.IdUsuario)).Select<GEN_USUARIO, int>((Expression<Func<GEN_USUARIO, int>>)(p => p.IdPuntoEntRec)).FirstOrDefault<int>());
    //                entity2.TipoPunto = 1;
    //                entity2.Problema = false;
    //                entity2.EstatusPuntosEntRec = false;
    //                try
    //                {
    //                    this.db.OPE_SOLICITUDPUNTOSENTREC.Add(entity2);
    //                    this.db.SaveChanges();
    //                    resultados.Result = true;
    //                }
    //                catch (Exception ex)
    //                {
    //                    resultados.Result = false;
    //                    resultados.Mensaje = "No se pudo Registrar Punto Remitente: " + ex.Message;
    //                }
    //                if (resultados.Result)
    //                {
    //                    entity2.IdSolicitud = resultados.Valor;
    //                    entity2.IdPuntoEntRec = new int?();
    //                    entity2.TipoPunto = 2;
    //                    entity2.DescripcionPuntoEntRec = Destinatario;
    //                    entity2.DirCompletaPuntoEntRec = Direccion;
    //                    entity2.Problema = false;
    //                    entity2.EstatusPuntosEntRec = false;
    //                    try
    //                    {
    //                        this.db.OPE_SOLICITUDPUNTOSENTREC.Add(entity2);
    //                        this.db.SaveChanges();
    //                        resultados.Result = true;
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        resultados.Result = false;
    //                        resultados.Mensaje = "No se pudo Registrar Punto Entrega: " + ex.Message;
    //                    }
    //                }
    //            }
    //        }
    //        return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
    //    }
    //}
}