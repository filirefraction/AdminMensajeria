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
    public class PuntosEntRecController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: PuntosEntRec
        public ActionResult Index(int id)
        {
            var oPE_PUNTOENTREC = db.OPE_PUNTOENTREC.Where(x => x.IdClientProvee == id);
            return View(oPE_PUNTOENTREC.ToList());
        }

        public ActionResult Puntos(int id)
        {
            ViewBag.idclientprovee = id;
            return PartialView("~/Views/PuntosEntRec/Puntos.cshtml");
        }

        // GET: PuntosEntRec/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_PUNTOENTREC oPE_PUNTOENTREC = db.OPE_PUNTOENTREC.Find(id);
            if (oPE_PUNTOENTREC == null)
            {
                return HttpNotFound();
            }
            return View(oPE_PUNTOENTREC);
        }

        // GET: PuntosEntRec/Create
        public ActionResult Create(int id)
        {
            ViewBag.idclientprovee = id;
            return PartialView("~/Views/PuntosEntRec/Create.cshtml");
        }

        public JsonResult CreatePuntos(OPE_PUNTOENTREC Punto)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.OPE_PUNTOENTREC.Add(Punto);
                db.SaveChanges();
                resultado.Result = true;
            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        // GET: PuntosEntRec/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_PUNTOENTREC oPE_PUNTOENTREC = db.OPE_PUNTOENTREC.Find(id);
            if (oPE_PUNTOENTREC == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdColonia = new SelectList(db.GEN_COLONIA.Where(x => x.IdCodigoP == oPE_PUNTOENTREC.GEN_COLONIA.IdCodigoP), "IdColonia", "Colonia", oPE_PUNTOENTREC.IdColonia);
            return PartialView("~/Views/PuntosEntRec/Edit.cshtml",oPE_PUNTOENTREC);
        }

        public JsonResult EditPuntos(OPE_PUNTOENTREC Punto)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(Punto).State = EntityState.Modified;
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

        public JsonResult DeletePunto(int id)
        {
            Resultados results = new Resultados();
            OPE_PUNTOENTREC Puntos = db.OPE_PUNTOENTREC.Find(id);

            try
            {
                db.OPE_PUNTOENTREC.Remove(Puntos);
                db.SaveChanges();
                results.Result = true;
                results.Mensaje = "Punto Eliminado Correctamente";
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
