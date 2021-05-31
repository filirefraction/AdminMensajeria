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

        public ActionResult Index() => (ActionResult)this.View((object)this.db.OPE_CATEGORIA.ToList<OPE_CATEGORIA>());

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OPE_CATEGORIA opeCategoria = this.db.OPE_CATEGORIA.Find(new object[1]
            {
        (object) id
            });
            return opeCategoria == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)opeCategoria);
        }

        public ActionResult Create() => (ActionResult)this.View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCategoria,Categoria")] OPE_CATEGORIA oPE_CATEGORIA)
        {
            if (!this.ModelState.IsValid)
                return (ActionResult)this.View((object)oPE_CATEGORIA);
            this.db.OPE_CATEGORIA.Add(oPE_CATEGORIA);
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OPE_CATEGORIA opeCategoria = this.db.OPE_CATEGORIA.Find(new object[1]
            {
        (object) id
            });
            return opeCategoria == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)opeCategoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCategoria,Categoria")] OPE_CATEGORIA oPE_CATEGORIA)
        {
            if (!this.ModelState.IsValid)
                return (ActionResult)this.View((object)oPE_CATEGORIA);
            this.db.Entry<OPE_CATEGORIA>(oPE_CATEGORIA).State = EntityState.Modified;
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            OPE_CATEGORIA opeCategoria = this.db.OPE_CATEGORIA.Find(new object[1]
            {
        (object) id
            });
            return opeCategoria == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)opeCategoria);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.db.OPE_CATEGORIA.Remove(this.db.OPE_CATEGORIA.Find(new object[1]
            {
        (object) id
            }));
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("Index");
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
