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
    public class CategoriaController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Categoria
        public ActionResult Index()
        {
            return View(db.OPE_CATEGORIA.ToList());
        }

        // GET: Categoria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_CATEGORIA oPE_CATEGORIA = db.OPE_CATEGORIA.Find(id);
            if (oPE_CATEGORIA == null)
            {
                return HttpNotFound();
            }
            return View(oPE_CATEGORIA);
        }

        // GET: Categoria/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCategoria,Categoria")] OPE_CATEGORIA oPE_CATEGORIA)
        {
            if (ModelState.IsValid)
            {
                db.OPE_CATEGORIA.Add(oPE_CATEGORIA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oPE_CATEGORIA);
        }

        // GET: Categoria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_CATEGORIA oPE_CATEGORIA = db.OPE_CATEGORIA.Find(id);
            if (oPE_CATEGORIA == null)
            {
                return HttpNotFound();
            }
            return View(oPE_CATEGORIA);
        }

        // POST: Categoria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCategoria,Categoria")] OPE_CATEGORIA oPE_CATEGORIA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oPE_CATEGORIA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oPE_CATEGORIA);
        }

        // GET: Categoria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_CATEGORIA oPE_CATEGORIA = db.OPE_CATEGORIA.Find(id);
            if (oPE_CATEGORIA == null)
            {
                return HttpNotFound();
            }
            return View(oPE_CATEGORIA);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OPE_CATEGORIA oPE_CATEGORIA = db.OPE_CATEGORIA.Find(id);
            db.OPE_CATEGORIA.Remove(oPE_CATEGORIA);
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
