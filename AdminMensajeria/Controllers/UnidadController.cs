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
    public class UnidadController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Unidad
        public ActionResult Index()
        {
            return View(db.GEN_UNIDAD.ToList());
        }

        // GET: Unidad/Create
        public ActionResult Create()
        {
            return PartialView("~/Views/Unidad/Create.cshtml");
        }

        public JsonResult CreateUnidad(GEN_UNIDAD Unidad)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.GEN_UNIDAD.Add(Unidad);
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


        // GET: Unidad/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GEN_UNIDAD gEN_UNIDAD = db.GEN_UNIDAD.Find(id);
            if (gEN_UNIDAD == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Unidad/Edit.cshtml", gEN_UNIDAD);

        }

        public JsonResult EditUnidad(GEN_UNIDAD Unidad)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(Unidad).State = EntityState.Modified;
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

        public JsonResult DeleteUnidad(int id)
        {
            Resultados results = new Resultados();
            GEN_UNIDAD Unidad = db.GEN_UNIDAD.Find(id);

            try
            {
                db.GEN_UNIDAD.Remove(Unidad);
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
