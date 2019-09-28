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
    public class EstadoController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Estado
        public ActionResult Index()
        {
            ViewBag.ListPais = new SelectList(db.GEN_PAIS, "IdPais", "Pais");
            return View();
        } 

        // GET: Estado
        public ActionResult Estados(int id)
        {
            var gEN_ESTADO = db.GEN_ESTADO.Where(x => x.IdPais == id);
            return PartialView(gEN_ESTADO.ToList());
        }
        
        // GET: Estado/Create
        public ActionResult Create(int id)
        {
            ViewBag.pais = id;

            ViewBag.IdPais = new SelectList(db.GEN_PAIS, "IdPais", "Pais");
            return PartialView("~/Views/Estado/Create.cshtml");
        }

        public JsonResult CreateEstado(GEN_ESTADO gEN_Estado)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.GEN_ESTADO.Add(gEN_Estado);
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

        // GET: Estado/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.idestado = id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GEN_ESTADO gEN_ESTADO = db.GEN_ESTADO.Find(id);
            if (gEN_ESTADO == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPais = new SelectList(db.GEN_PAIS, "IdPais", "Pais", gEN_ESTADO.IdPais);

            return PartialView("~/Views/Estado/Edit.cshtml", gEN_ESTADO);
        }

        public JsonResult EditEstado(GEN_ESTADO gEN_Estado)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(gEN_Estado).State = EntityState.Modified;
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
        

        public JsonResult DeleteEstado(int id)
        {
            Resultados results = new Resultados();
            GEN_ESTADO Estado = db.GEN_ESTADO.Find(id);

            try
            {
                db.GEN_ESTADO.Remove(Estado);
                db.SaveChanges();
                results.Result = true;
                results.Mensaje = "Estado Eliminado Correctamente";
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
