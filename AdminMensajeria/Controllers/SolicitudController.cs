using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminMensajeria.Models;
//using ZXing;
//using ZXing.Common;
using System.Drawing;
using System.Drawing.Imaging;
using AdminMensajeria.Clases;
using System.Linq.Expressions;

namespace AdminMensajeria.Controllers
{
    public class SolicitudController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Solicitud
        public ActionResult Index()
        {
           return View();
        }

        public ActionResult Solicitudes(DateTime Desde, DateTime Hasta)
        {
            Hasta = Hasta.AddDays(1);

            int IdUsuario;
            if (Session["IdUsuario"] == null)
            {
                ViewBag.idusuario = 0;
                ViewBag.tipousuario = 0;
                ViewBag.desde = Desde;
                ViewBag.hasta = Hasta;
            }
            else
            {
                IdUsuario = (int)Session["IdUsuario"];
                ViewBag.idusuario = IdUsuario;
                ViewBag.tipousuario = (from u in db.GEN_USUARIO where u.IdUsuario == IdUsuario select u.TipoUsuario).FirstOrDefault();
                ViewBag.desde = Desde;
                ViewBag.hasta = Hasta;
            }
            return PartialView("~/Views/Solicitud/Solicitudes.cshtml");
        }


        // GET: Solicitud/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            if (oPE_SOLICITUD == null)
            {
                return HttpNotFound();
            }
            return View(oPE_SOLICITUD);
        }

        // GET: Solicitud/Create
        public ActionResult Create()
        {
            int IdUsuario = 0;
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Index", "Home");
            else
                IdUsuario = (int)Session["IdUsuario"];


            int idusuario = IdUsuario; //aqui va la variable de sesión
            ViewBag.idpuntorec = (from u in db.GEN_USUARIO where u.IdUsuario == idusuario select u.IdPuntoEntRec).FirstOrDefault();
            ViewBag.IdUnidad = new SelectList(db.GEN_UNIDAD, "IdUnidad", "DescripcionUnidad");
            ViewBag.ListCategoria = new SelectList(db.OPE_CATEGORIA, "IdCategoria", "Categoria");
            return View();
        }

        public JsonResult CreateSolicitud(OPE_SOLICITUD Solicitud)
        {
            Resultados resultados = new Resultados();
            if (this.Session["IdUsuario"] == null)
            {
                resultados.Redirecciona = true;
                return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
            }
            int num = (int)this.Session["IdUsuario"];
            Solicitud.IdUsuario = num;
            Solicitud.FechaCreacion = helper.dateTimeZone(DateTime.Now);
            Solicitud.Emergencia = false;
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
                resultados.Mensaje = ex.Message;
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateProducto(OPE_SOLICITUDPRODUCTO Producto)
        {
            Resultados resultados = new Resultados();
            try
            {
                this.db.OPE_SOLICITUDPRODUCTO.Add(Producto);
                this.db.SaveChanges();
                resultados.Result = true;
            }
            catch (Exception ex)
            {
                resultados.Result = false;
                resultados.Mensaje = ex.Message;
                resultados.Valor = 0;
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateSolicitudPunto(OPE_SOLICITUDPUNTOSENTREC Punto)
        {
            Resultados resultados = new Resultados();
            try
            {
                this.db.OPE_SOLICITUDPUNTOSENTREC.Add(Punto);
                this.db.SaveChanges();
                resultados.Result = true;
            }
            catch (Exception ex)
            {
                resultados.Result = false;
                resultados.Mensaje = ex.Message;
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Puntos(int id)
        {
            ViewBag.idclientprovee = id;
            return PartialView("~/Views/Solicitud/Puntos.cshtml");
        }

        public ActionResult Rastreo(string id, int ctlFolio)
        {
            if (ctlFolio == 0)
                return (ActionResult)this.PartialView("~/Views/Solicitud/Rastreo.cshtml", (object)this.db.OPE_SOLICITUD.Find(new object[1]
                {
          (object) Convert.ToInt32(id)
                }));
            return (ActionResult)this.PartialView("~/Views/Solicitud/Rastreo.cshtml", (object)this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(c => c.Folio == id)).FirstOrDefault<OPE_SOLICITUD>());
        }

        public ActionResult Recepcion()
        {
            if (this.db.SIS_CONFIG.Where<SIS_CONFIG>((Expression<Func<SIS_CONFIG, bool>>)(u => u.IdConfig == 1)).Select<SIS_CONFIG, bool>((Expression<Func<SIS_CONFIG, bool>>)(u => u.AplicaConfig)).FirstOrDefault<bool>())
                ViewBag.ctrlFolio = 1;
            else
                ViewBag.ctrlFolio = 0;

            return PartialView("~/Views/Solicitud/Recepcion.cshtml");
        }
        public JsonResult RecibirPaquete(string id, int ctlFolio)
        {
            Resultados resultados = new Resultados();
            if (this.Session["IdUsuario"] == null)
            {
                resultados.Redirecciona = true;
                return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
            }
            int IdUsuario = (int)this.Session["IdUsuario"];
            string str = this.db.GEN_USUARIO.Where<GEN_USUARIO>((Expression<Func<GEN_USUARIO, bool>>)(n => n.IdUsuario == IdUsuario)).Select<GEN_USUARIO, string>((Expression<Func<GEN_USUARIO, string>>)(n => n.NombreUsuario)).FirstOrDefault<string>();
            OPE_SOLICITUD Solicitud = this.db.OPE_SOLICITUD.FirstOrDefault<OPE_SOLICITUD>();
            this.db.OPE_SOLICITUDPRODUCTO.FirstOrDefault<OPE_SOLICITUDPRODUCTO>();
            this.db.OPE_SOLICITUDPUNTOSENTREC.FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
            OPE_SOLICITUDPRODUCTO entity1;
            OPE_SOLICITUDPUNTOSENTREC entity2;
            if (ctlFolio == 0)
            {
                int idi = Convert.ToInt32(id);
                Solicitud = this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.IdSolicitud == idi)).FirstOrDefault<OPE_SOLICITUD>();
                entity1 = this.db.OPE_SOLICITUDPRODUCTO.Where<OPE_SOLICITUDPRODUCTO>((Expression<Func<OPE_SOLICITUDPRODUCTO, bool>>)(x => x.IdSolicitud == idi)).FirstOrDefault<OPE_SOLICITUDPRODUCTO>();
                entity2 = this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(x => x.IdSolicitud == idi && x.TipoPunto == 2)).FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
            }
            else
            {
                Solicitud = this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.Folio == id)).FirstOrDefault<OPE_SOLICITUD>();
                if (Solicitud != null)
                {
                    entity1 = this.db.OPE_SOLICITUDPRODUCTO.Where<OPE_SOLICITUDPRODUCTO>((Expression<Func<OPE_SOLICITUDPRODUCTO, bool>>)(x => x.IdSolicitud == Solicitud.IdSolicitud)).FirstOrDefault<OPE_SOLICITUDPRODUCTO>();
                    entity2 = this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(x => x.IdSolicitud == Solicitud.IdSolicitud && x.TipoPunto == 2)).FirstOrDefault<OPE_SOLICITUDPUNTOSENTREC>();
                }
                else
                {
                    entity2 = (OPE_SOLICITUDPUNTOSENTREC)null;
                    entity1 = (OPE_SOLICITUDPRODUCTO)null;
                }
            }
            if (entity2 != null)
            {
                DateTime dateTime = helper.dateTimeZone(DateTime.Now);
                entity2.FechaProgramada = dateTime.Hour >= 10 ? new DateTime?(dateTime.AddDays(1.0)) : new DateTime?(dateTime);
                this.db.Entry<OPE_SOLICITUDPUNTOSENTREC>(entity2).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            if (entity1 != null)
            {
                if (!entity1.Recibido)
                {
                    entity1.Recibido = true;
                    entity1.FechaRecepcion = new DateTime?(helper.dateTimeZone(DateTime.Now));
                    entity1.Receptor = str;
                    Solicitud.EstatusSolicitud = 2;
                    try
                    {
                        this.db.Entry<OPE_SOLICITUDPRODUCTO>(entity1).State = EntityState.Modified;
                        this.db.Entry<OPE_SOLICITUD>(Solicitud).State = EntityState.Modified;
                        this.db.SaveChanges();
                        resultados.Result = true;
                        resultados.Mensaje = "El producto de la solicitud " + id.ToString() + " se recibio correctamente " + entity1.Descripcion;
                    }
                    catch (Exception ex)
                    {
                        resultados.Result = false;
                        resultados.Mensaje = "Error al recibir producto: " + ex.Message;
                    }
                }
                else
                {
                    resultados.Result = false;
                    resultados.Mensaje = "El producto de la solicitud " + id.ToString() + " se Recibió anteriormente";
                }
            }
            else
            {
                resultados.Result = false;
                resultados.Mensaje = "No Existe Numero de Envío";
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportaEntrega(int id)
        {
            ViewBag.idsolicitud = id;
            return PartialView("~/Views/Solicitud/ReportaEntrega.cshtml");
        }
        public JsonResult ReportarEntrega(int id, DateTime FechaEntrega)
        {
            Resultados resultado = new Resultados();
            OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Find(id);
            OPE_SOLICITUDPUNTOSENTREC Entrega = db.OPE_SOLICITUDPUNTOSENTREC.Where(x => x.IdSolicitud == id && x.TipoPunto == 2).FirstOrDefault();

            try
            {
                Solicitud.EstatusSolicitud = 4;
                Entrega.FechaReal = FechaEntrega;
                db.Entry(Solicitud).State = EntityState.Modified;
                db.Entry(Entrega).State = EntityState.Modified;
                db.SaveChanges();
                resultado.Result = true;

            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = "Error al Reportar Entrega Envío: " + ex.Message;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CancelaEnvio(int id)
        {
            Resultados resultado = new Resultados();
            OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Find(id);
            Solicitud.EstatusSolicitud = 5;
            try
            {
                db.Entry(Solicitud).State = EntityState.Modified;
                db.SaveChanges();
                resultado.Result = true;

            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = "Error al Cancelar Envío: " + ex.Message;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        // GET: Solicitud/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            if (oPE_SOLICITUD == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.GEN_USUARIO, "IdUsuario", "NombreUsuario", oPE_SOLICITUD.IdUsuario);
            return View(oPE_SOLICITUD);
        }

        // POST: Solicitud/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSolicitud,IdUsuario,IdClientProvee,Folio,FechaCreacion,RequiereAcuse,Acuse,Emergencia,EstatusSolicitud")] OPE_SOLICITUD oPE_SOLICITUD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oPE_SOLICITUD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.GEN_USUARIO, "IdUsuario", "NombreUsuario", oPE_SOLICITUD.IdUsuario);
            return View(oPE_SOLICITUD);
        }

        // GET: Solicitud/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            if (oPE_SOLICITUD == null)
            {
                return HttpNotFound();
            }
            return View(oPE_SOLICITUD);
        }

        // POST: Solicitud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            db.OPE_SOLICITUD.Remove(oPE_SOLICITUD);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult ListaClientes(int id)
        {
            var Clientes = (from e in db.OPE_CLIENTPROVEE
                            where e.IdCategoria == id
                            select new
                            {
                                idcliente = e.IdClientProvee,
                                cliente = e.RazonSocial
                            }).ToList();

            return Json(Clientes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InformacionPunto(int id)
        {
            var Punto = (from p in db.OPE_PUNTOENTREC
                         where p.IdPuntoEntRec == id
                         select new
                         {
                             idpunto = p.IdPuntoEntRec,
                             descripcion = p.DescripcionPunto,
                             contacto = p.ContactoPunto,
                             calle = p.CallePunto,
                             noext = p.NoExtPunto,
                             noint = p.NoIntPunto,
                             referencia = p.Referencia,
                             cp = p.GEN_COLONIA.GEN_CODIGOPOSTAL.CodigoPostal,
                             idcolonia = p.IdColonia,
                             colonia = p.GEN_COLONIA.Colonia,
                             municipio = p.GEN_COLONIA.GEN_MUNICIPIO.Municipio,
                             estado = p.GEN_COLONIA.GEN_MUNICIPIO.GEN_ESTADO.CiudadEstado,
                             pais = p.GEN_COLONIA.GEN_MUNICIPIO.GEN_ESTADO.GEN_PAIS.Pais
                         }).FirstOrDefault();

            return Json(Punto, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaColonias(string id)
        {
            var Colonias = (from c in db.GEN_COLONIA
                            join p in db.GEN_CODIGOPOSTAL
                            on c.IdCodigoP equals p.IdCodigoP
                            where p.CodigoPostal == id
                            select new
                            {
                                idcolonia = c.IdColonia,
                                colonia = c.Colonia
                            }).ToList();

            return Json(Colonias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ComplementoDomicilio(int id)
        {
            var Complemento = (from c in db.GEN_COLONIA
                               where c.IdColonia == id
                               select new
                               {
                                   municipio = c.GEN_MUNICIPIO.Municipio,
                                   estado = c.GEN_MUNICIPIO.GEN_ESTADO.CiudadEstado,
                                   pais = c.GEN_MUNICIPIO.GEN_ESTADO.GEN_PAIS.Pais
                               }).FirstOrDefault();

            return Json(Complemento, JsonRequestBehavior.AllowGet);
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
