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
    public class PaisController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Pais
        public ActionResult Index()
        {
            return View(db.GEN_PAIS.ToList());
        }

        // GET: Pais/Create
        public ActionResult Create()
        {
            return PartialView("~/Views/Pais/Create.cshtml");
        }

        public JsonResult CreatePais(GEN_PAIS gEN_Pais)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.GEN_PAIS.Add(gEN_Pais);
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

        // GET: Pais/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.idpais = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GEN_PAIS gEN_PAIS = db.GEN_PAIS.Find(id);
            if (gEN_PAIS == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Pais/Edit.cshtml", gEN_PAIS);
        }

        public JsonResult EditPais(GEN_PAIS gEN_Pais)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(gEN_Pais).State = EntityState.Modified;
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

        public JsonResult DeletePais(int id)
        {
            Resultados results = new Resultados();
            GEN_PAIS Pais = db.GEN_PAIS.Find(id);

            try
            {
                db.GEN_PAIS.Remove(Pais);
                db.SaveChanges();
                results.Result = true;
                results.Mensaje = "País Eliminado Correctamente";
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
