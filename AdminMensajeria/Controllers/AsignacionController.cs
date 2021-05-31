using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminMensajeria.Clases;
using AdminMensajeria.Models;

namespace AdminMensajeria.Controllers
{
    public class AsignacionController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Asignacion
        public ActionResult Index() => (ActionResult)this.View();

        public ActionResult Asignaciones(DateTime Desde, DateTime Hasta)
        {
            Hasta = Hasta.AddDays(1.0);

            ViewBag.desde = Desde;
            ViewBag.hasta = Hasta;


            return (ActionResult)this.PartialView("~/Views/Asignacion/Asignaciones.cshtml");
        }

        public ActionResult Entregas(int id)
        {
            ViewBag.idguia = id;
            if (this.db.SIS_CONFIG.Where<SIS_CONFIG>((Expression<Func<SIS_CONFIG, bool>>)(u => u.IdConfig == 1)).Select<SIS_CONFIG, bool>((Expression<Func<SIS_CONFIG, bool>>)(u => u.AplicaConfig)).FirstOrDefault<bool>())
            {
                ViewBag.ctrlFolio = 1;
            }
            else
            {
                ViewBag.ctrlFolio = 0;
            }

            return (ActionResult)this.PartialView();
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
            Asignacion.FechaCreacionGuia = helper.dateTimeZone(DateTime.Now);


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

            if (this.db.SIS_CONFIG.Where<SIS_CONFIG>((Expression<Func<SIS_CONFIG, bool>>)(u => u.IdConfig == 1)).Select<SIS_CONFIG, bool>((Expression<Func<SIS_CONFIG, bool>>)(u => u.AplicaConfig)).FirstOrDefault<bool>())
            {
                ViewBag.ctrlFolio = 1;
            }
            else
            {
                ViewBag.ctrlFolio = 0;
            }

            return PartialView("~/Views/Asignacion/AsignaReferencia.cshtml");
        }

        public JsonResult AsignarEntregaRef(string id, int IdGuia, string Referencia, int ctlFolio)
        {
            OPE_SOLICITUD Solicitud = this.db.OPE_SOLICITUD.FirstOrDefault<OPE_SOLICITUD>();
            this.db.OPE_SOLICITUDPUNTOSENTREC.FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
            this.db.OPE_SOLICITUDPRODUCTO.FirstOrDefault<OPE_SOLICITUDPRODUCTO>();
            OPE_SOLICITUDPUNTOSENTREC entity;
            OPE_SOLICITUDPRODUCTO solicitudproducto;
            if (ctlFolio == 0)
            {
                int idi = Convert.ToInt32(id);
                Solicitud = this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.IdSolicitud == idi)).FirstOrDefault<OPE_SOLICITUD>();
                entity = this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(x => x.IdSolicitud == idi && x.TipoPunto == 2)).FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
                solicitudproducto = this.db.OPE_SOLICITUDPRODUCTO.Where<OPE_SOLICITUDPRODUCTO>((Expression<Func<OPE_SOLICITUDPRODUCTO, bool>>)(x => x.IdSolicitud == idi)).FirstOrDefault<OPE_SOLICITUDPRODUCTO>();
            }
            else
            {
                Solicitud = this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.Folio == id)).FirstOrDefault<OPE_SOLICITUD>();
                if (Solicitud != null)
                {
                    solicitudproducto = this.db.OPE_SOLICITUDPRODUCTO.Where<OPE_SOLICITUDPRODUCTO>((Expression<Func<OPE_SOLICITUDPRODUCTO, bool>>)(x => x.IdSolicitud == Solicitud.IdSolicitud)).FirstOrDefault<OPE_SOLICITUDPRODUCTO>();
                    entity = this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(x => x.IdSolicitud == Solicitud.IdSolicitud && x.TipoPunto == 2)).FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
                }
                else
                {
                    entity = (OPE_SOLICITUDPUNTOSENTREC)null;
                    solicitudproducto = (OPE_SOLICITUDPRODUCTO)null;
                }
            }
            Resultados resultados = new Resultados();
            if (entity != null)
            {
                if (solicitudproducto.Recibido)
                {
                    if (!entity.IdGuia.HasValue)
                    {
                        Solicitud.EstatusSolicitud = 3;
                        entity.IdGuia = new int?(IdGuia);
                        entity.RefExtPuntosEntRec = Referencia;
                        try
                        {
                            this.db.Entry<OPE_SOLICITUD>(Solicitud).State = EntityState.Modified;
                            this.db.Entry<OPE_SOLICITUDPUNTOSENTREC>(entity).State = EntityState.Modified;
                            this.db.SaveChanges();
                            resultados.Result = true;
                            resultados.Mensaje = "El envío " + id.ToString() + " se Asigno correctamente " + solicitudproducto.Descripcion;
                        }
                        catch (Exception ex)
                        {
                            resultados.Result = false;
                            resultados.Mensaje = "Error al Asignar Envío: " + ex.Message;
                        }
                    }
                    else
                    {
                        resultados.Result = false;
                        resultados.Mensaje = "El envío " + id.ToString() + " fue asignada anteriormente al Número: " + entity.IdGuia.ToString();
                    }
                }
                else
                {
                    resultados.Result = false;
                    resultados.Mensaje = "Este Producto no se ha recibido en ventanilla, debe recibirlo primero";
                }
            }
            else
            {
                resultados.Result = false;
                resultados.Mensaje = "No Existe Numero de Envío";
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AsignarEntrega(string id, int IdGuia, int ctlFolio)
        {
            OPE_SOLICITUD Solicitud = this.db.OPE_SOLICITUD.FirstOrDefault<OPE_SOLICITUD>();
            this.db.OPE_SOLICITUDPUNTOSENTREC.FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
            this.db.OPE_SOLICITUDPRODUCTO.FirstOrDefault<OPE_SOLICITUDPRODUCTO>();
            OPE_SOLICITUDPUNTOSENTREC entity;
            OPE_SOLICITUDPRODUCTO solicitudproducto;
            if (ctlFolio == 0)
            {
                int idi = Convert.ToInt32(id);
                Solicitud = this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.IdSolicitud == idi)).FirstOrDefault<OPE_SOLICITUD>();
                entity = this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(x => x.IdSolicitud == idi && x.TipoPunto == 2)).FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
                solicitudproducto = this.db.OPE_SOLICITUDPRODUCTO.Where<OPE_SOLICITUDPRODUCTO>((Expression<Func<OPE_SOLICITUDPRODUCTO, bool>>)(x => x.IdSolicitud == idi)).FirstOrDefault<OPE_SOLICITUDPRODUCTO>();
            }
            else
            {
                Solicitud = this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.Folio == id)).FirstOrDefault<OPE_SOLICITUD>();
                if (Solicitud != null)
                {
                    solicitudproducto = this.db.OPE_SOLICITUDPRODUCTO.Where<OPE_SOLICITUDPRODUCTO>((Expression<Func<OPE_SOLICITUDPRODUCTO, bool>>)(x => x.IdSolicitud == Solicitud.IdSolicitud)).FirstOrDefault<OPE_SOLICITUDPRODUCTO>();
                    entity = this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(x => x.IdSolicitud == Solicitud.IdSolicitud && x.TipoPunto == 2)).FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
                }
                else
                {
                    entity = (OPE_SOLICITUDPUNTOSENTREC)null;
                    solicitudproducto = (OPE_SOLICITUDPRODUCTO)null;
                }
            }
            Resultados resultados = new Resultados();
            if (entity != null)
            {
                if (solicitudproducto.Recibido)
                {
                    if (!entity.IdGuia.HasValue)
                    {
                        Solicitud.EstatusSolicitud = 3;
                        entity.IdGuia = new int?(IdGuia);
                        try
                        {
                            this.db.Entry<OPE_SOLICITUD>(Solicitud).State = EntityState.Modified;
                            this.db.Entry<OPE_SOLICITUDPUNTOSENTREC>(entity).State = EntityState.Modified;
                            this.db.SaveChanges();
                            resultados.Result = true;
                            resultados.Mensaje = "El envío " + id.ToString() + " se Asigno correctamente " + solicitudproducto.Descripcion;
                        }
                        catch (Exception ex)
                        {
                            resultados.Result = false;
                            resultados.Mensaje = "Error al Asignar Envío: " + ex.Message;
                        }
                    }
                    else
                    {
                        resultados.Result = false;
                        resultados.Mensaje = "Este Envío ya se Asigno al Número: " + entity.IdGuia.ToString();
                    }
                }
                else
                {
                    resultados.Result = false;
                    resultados.Mensaje = "Este Producto no se ha recibido en ventanilla, debe recibirlo primero";
                }
            }
            else
            {
                resultados.Result = false;
                resultados.Mensaje = "No Existe Numero de Envío";
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QuitarEntrega(int id)
        {
            OPE_SOLICITUDPUNTOSENTREC entity1 = this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(x => x.IdSolicitudPuntosEntRec == id)).FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
            OPE_SOLICITUD entity2 = this.db.OPE_SOLICITUD.Find(new object[1]
            {
        (object) entity1.IdSolicitud
            });
            Resultados resultados = new Resultados();
            entity1.IdGuia = new int?();
            if (entity2.EstatusSolicitud > 3)
            {
                resultados.Result = false;
                resultados.Mensaje = "No Puede Quitar Esta Asignación el Envio Tiene un Reporte de Entrega";
            }
            else
            {
                try
                {
                    entity2.EstatusSolicitud = 2;
                    this.db.Entry<OPE_SOLICITUDPUNTOSENTREC>(entity1).State = EntityState.Modified;
                    this.db.Entry<OPE_SOLICITUD>(entity2).State = EntityState.Modified;
                    this.db.SaveChanges();
                    resultados.Result = true;
                    resultados.Mensaje = "Se quito en envío";
                }
                catch (Exception ex)
                {
                    resultados.Result = false;
                    resultados.Mensaje = "Error al Quitar Envío: " + ex.Message;
                }
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
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
            Resultados resultados = new Resultados();
            OPE_GUIA entity1 = this.db.OPE_GUIA.Find(id);
            List<OPE_SOLICITUDPUNTOSENTREC> list = this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(x => x.IdGuia == (int?)id)).ToList<OPE_SOLICITUDPUNTOSENTREC>();
            if (list != null)
            {
                foreach (OPE_SOLICITUDPUNTOSENTREC entity2 in list)
                {
                    OPE_SOLICITUD entity3 = this.db.OPE_SOLICITUD.Find(new object[1]
                    {
            (object) entity2.IdSolicitud
                    });
                    entity2.IdGuia = new int?();
                    entity2.Problema = false;
                    entity2.ObsProblema = (string)null;
                    entity2.FotoPuntosEntRec = (string)null;
                    entity2.FirmaPuntosEntRec = (string)null;
                    entity2.Latitud = (string)null;
                    entity2.Longitud = (string)null;
                    entity3.EstatusSolicitud = 2;
                    try
                    {
                        this.db.Entry<OPE_SOLICITUDPUNTOSENTREC>(entity2).State = EntityState.Modified;
                        this.db.Entry<OPE_SOLICITUD>(entity3).State = EntityState.Modified;
                        this.db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            try
            {
                this.db.OPE_GUIA.Remove(entity1);
                this.db.SaveChanges();
                resultados.Result = true;
                resultados.Mensaje = "Asignacion Eliminado Correctamente";
            }
            catch (Exception ex)
            {
                resultados.Result = false;
                resultados.Mensaje = ex.Message;
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
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
