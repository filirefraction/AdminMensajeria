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

        // GET: Unidad/Details/5
        public ActionResult Details(int? id)
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
            return View(gEN_UNIDAD);
        }

        // GET: Unidad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Unidad/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUnidad,DescripcionUnidad,Codigo,Simbolo,EstatusUnidad")] GEN_UNIDAD gEN_UNIDAD)
        {
            if (ModelState.IsValid)
            {
                db.GEN_UNIDAD.Add(gEN_UNIDAD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gEN_UNIDAD);
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
            return View(gEN_UNIDAD);
        }

        // POST: Unidad/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUnidad,DescripcionUnidad,Codigo,Simbolo,EstatusUnidad")] GEN_UNIDAD gEN_UNIDAD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gEN_UNIDAD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gEN_UNIDAD);
        }

        // GET: Unidad/Delete/5
        public ActionResult Delete(int? id)
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
            return View(gEN_UNIDAD);
        }

        // POST: Unidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GEN_UNIDAD gEN_UNIDAD = db.GEN_UNIDAD.Find(id);
            db.GEN_UNIDAD.Remove(gEN_UNIDAD);
            db.SaveChanges();
            return RedirectToAction("Index");
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
