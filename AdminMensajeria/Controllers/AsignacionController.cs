using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminMensajeria.Models;

namespace AdminMensajeria.Controllers
{
    public class AsignacionController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Asignacion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Asignaciones(DateTime Desde, DateTime Hasta)
        {
            Hasta = Hasta.AddDays(1);
            ViewBag.desde = Desde;
            ViewBag.hasta = Hasta;
            return PartialView("~/Views/Asignacion/Asignaciones.cshtml");
        }


        public ActionResult Entregas(int id)
        {
            ViewBag.idguia = id;
            return PartialView();
        }

        // GET: Asignacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_GUIA oPE_GUIA = db.OPE_GUIA.Find(id);
            if (oPE_GUIA == null)
            {
                return HttpNotFound();
            }
            return View(oPE_GUIA);
        }

        // GET: Asignacion/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.GEN_USUARIO.Where(x => x.TipoUsuario == 4), "IdUsuario", "NombreUsuario");
            return PartialView("~/Views/Asignacion/Create.cshtml");
        }

        public JsonResult CreateAsignacion(OPE_GUIA Asignacion)
        {
            Resultados resultado = new Resultados();
            Asignacion.FechaCreacionGuia = DateTime.Now; //Local
            //Asignacion.FechaCreacionGuia = DateTime.Now.AddHours(1); //Producción


            try
            {
                db.OPE_GUIA.Add(Asignacion);
                db.SaveChanges();
                resultado.Result = true;
                resultado.Valor = (from a in db.OPE_GUIA orderby a.IdGuia descending select a.IdGuia).First();
            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
                resultado.Valor = 0;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        // GET: Asignacion/Create
        public ActionResult Referencia(int id)
        {
            ViewBag.IdSolicitudPunto = id;
            ViewBag.Referencia = (from r in db.OPE_SOLICITUDPUNTOSENTREC where r.IdSolicitudPuntosEntRec == id select r.RefExtPuntosEntRec).FirstOrDefault();
            return PartialView("~/Views/Asignacion/Referencia.cshtml");
        }

        public JsonResult EditReferencia(int id, string ReferenciaExt)
        {
            Resultados resultado = new Resultados();
            OPE_SOLICITUDPUNTOSENTREC Punto = db.OPE_SOLICITUDPUNTOSENTREC.Find(id);
            ViewBag.IdSolicitudPunto = id;

            Punto.RefExtPuntosEntRec = ReferenciaExt;

            try
            {
                db.Entry(Punto).State = EntityState.Modified;
                db.SaveChanges();
                resultado.Result = true;
                resultado.Opcional = Punto.IdGuia;
            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        // GET: Asignacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_GUIA oPE_GUIA = db.OPE_GUIA.Find(id);
            if (oPE_GUIA == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.GEN_USUARIO.Where(x => x.TipoUsuario == 4), "IdUsuario", "NombreUsuario", oPE_GUIA.IdUsuario);
            return View(oPE_GUIA);
        }

        public JsonResult EditAsignacion(OPE_GUIA Asignacion)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(Asignacion).State = EntityState.Modified;
                db.SaveChanges();
                resultado.Result = true;
            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
                resultado.Valor = 0;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AsignaReferencia(int id)
        {
            ViewBag.idguia = id;
            return PartialView("~/Views/Asignacion/AsignaReferencia.cshtml");
        }

        public JsonResult AsignarEntregaRef(int id, int IdGuia, string Referencia)
        {
            OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Where(x => x.IdSolicitud == id).FirstOrDefault();
            OPE_SOLICITUDPUNTOSENTREC Entrega = db.OPE_SOLICITUDPUNTOSENTREC.Where(x => x.IdSolicitud == id && x.TipoPunto == 2).FirstOrDefault();
            OPE_SOLICITUDPRODUCTO Producto = db.OPE_SOLICITUDPRODUCTO.Where(x => x.IdSolicitud == id).FirstOrDefault();


            Resultados resultado = new Resultados();

            if (Entrega != null)
            {
                if (Producto.Recibido == true)
                {
                    if (Entrega.IdGuia == null)
                    {
                        Solicitud.EstatusSolicitud = 3;
                        Entrega.IdGuia = IdGuia;
                        Entrega.RefExtPuntosEntRec = Referencia;

                        try
                        {
                            db.Entry(Solicitud).State = EntityState.Modified;
                            db.Entry(Entrega).State = EntityState.Modified;
                            db.SaveChanges();
                            resultado.Result = true;
                            resultado.Mensaje = "El envío " + id.ToString() + " se Asigno correctamente " + Producto.Descripcion;

                        }
                        catch (Exception ex)
                        {
                            resultado.Result = false;
                            resultado.Mensaje = "Error al Asignar Envío: " + ex.Message;
                        }
                    }
                    else
                    {
                        resultado.Result = false;
                        resultado.Mensaje = "El envío " + id.ToString() + " fue asignada anteriormente al Número: " + Entrega.IdGuia.ToString();
                    }
                }
                else
                {
                    resultado.Result = false;
                    resultado.Mensaje = "Este Producto no se ha recibido en ventanilla, debe recibirlo primero";
                }

            }
            else
            {
                resultado.Result = false;
                resultado.Mensaje = "No Existe Numero de Envío";
            }


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AsignarEntrega(int id, int IdGuia)
        {
            OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Where(x => x.IdSolicitud == id).FirstOrDefault();
            OPE_SOLICITUDPUNTOSENTREC Entrega = db.OPE_SOLICITUDPUNTOSENTREC.Where(x => x.IdSolicitud == id && x.TipoPunto == 2).FirstOrDefault();
            OPE_SOLICITUDPRODUCTO Producto = db.OPE_SOLICITUDPRODUCTO.Where(x => x.IdSolicitud == id).FirstOrDefault();


            Resultados resultado = new Resultados();

            if (Entrega != null)
            {
                if (Producto.Recibido == true)
                {
                    if (Entrega.IdGuia == null)
                    {
                        Solicitud.EstatusSolicitud = 3;
                        Entrega.IdGuia = IdGuia;

                        try
                        {
                            db.Entry(Solicitud).State = EntityState.Modified;
                            db.Entry(Entrega).State = EntityState.Modified;
                            db.SaveChanges();
                            resultado.Result = true;
                            resultado.Mensaje = "El envío " + id.ToString() + " se Asigno correctamente " + Producto.Descripcion;

                        }
                        catch (Exception ex)
                        {
                            resultado.Result = false;
                            resultado.Mensaje = "Error al Asignar Envío: " + ex.Message;
                        }
                    }
                    else
                    {
                        resultado.Result = false;
                        resultado.Mensaje = "Este Envío ya se Asigno al Número: " + Entrega.IdGuia.ToString();
                    }
                }
                else
                {
                    resultado.Result = false;
                    resultado.Mensaje = "Este Producto no se ha recibido en ventanilla, debe recibirlo primero";
                }

            }
            else
            {
                resultado.Result = false;
                resultado.Mensaje = "No Existe Numero de Envío";
            }


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public JsonResult QuitarEntrega(int id)
        {
            OPE_SOLICITUDPUNTOSENTREC Entrega = db.OPE_SOLICITUDPUNTOSENTREC.Where(x => x.IdSolicitudPuntosEntRec == id).FirstOrDefault();
            OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Find(Entrega.IdSolicitud);

            Resultados resultado = new Resultados();

            Entrega.IdGuia = null;

            if (Solicitud.EstatusSolicitud > 3)
            {
                resultado.Result = false;
                resultado.Mensaje = "No Puede Quitar Esta Asignación el Envio Tiene un Reporte de Entrega";
            }
            else
            {
                try
                {
                    Solicitud.EstatusSolicitud = 2;
                    db.Entry(Entrega).State = EntityState.Modified;
                    db.Entry(Solicitud).State = EntityState.Modified;
                    db.SaveChanges();
                    resultado.Result = true;
                    resultado.Mensaje = "Se quito en envío";

                }
                catch (Exception ex)
                {
                    resultado.Result = false;
                    resultado.Mensaje = "Error al Quitar Envío: " + ex.Message;
                }
            }


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportaEntregas(int id)
        {
            ViewBag.idguia = id;
            return PartialView("~/Views/Asignacion/ReportaEntregas.cshtml");
        }

        public JsonResult ReportarEntregas(int id, DateTime FechaEntrega)
        {
            Resultados resultado = new Resultados();

            var ids = (from s in db.OPE_SOLICITUDPUNTOSENTREC where s.IdGuia == id select s.IdSolicitud).ToList();

            foreach (var item in ids)
            {
                OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Find(item);
                OPE_SOLICITUDPUNTOSENTREC Entrega = db.OPE_SOLICITUDPUNTOSENTREC.Where(x => x.IdSolicitud == item && x.TipoPunto == 2).FirstOrDefault();
                Solicitud.EstatusSolicitud = 4;
                Entrega.FechaReal = FechaEntrega;
                db.Entry(Solicitud).State = EntityState.Modified;
                db.Entry(Entrega).State = EntityState.Modified;
                db.SaveChanges();
            }

            resultado.Result = true;


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteAsignacion(int id)
        {
            Resultados results = new Resultados();
            OPE_GUIA Asignacion = db.OPE_GUIA.Find(id);

            var Asignados = db.OPE_SOLICITUDPUNTOSENTREC.Where(x => x.IdGuia == id).ToList();

            if (Asignados != null)
            {
                foreach (var item in Asignados)
                {
                    OPE_SOLICITUDPUNTOSENTREC Entrega = item;
                    OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Find(Entrega.IdSolicitud);
                    Entrega.IdGuia = null;
                    Entrega.Problema = false;
                    Entrega.ObsProblema = null;
                    Entrega.FotoPuntosEntRec = null;
                    Entrega.FirmaPuntosEntRec = null;
                    Entrega.Latitud = null;
                    Entrega.Longitud = null;
                    Solicitud.EstatusSolicitud = 2;
                    try
                    {
                        db.Entry(Entrega).State = EntityState.Modified;
                        db.Entry(Solicitud).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                    }

                }
            }



            try
            {
                db.OPE_GUIA.Remove(Asignacion);
                db.SaveChanges();
                results.Result = true;
                results.Mensaje = "Asignacion Eliminado Correctamente";
            }
            catch (Exception ex)
            {
                results.Result = false;
                results.Mensaje = ex.Message;
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
