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
    public class CorreoController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Correo
        public ActionResult Index()
        {
            return View(db.GEN_EMAILCONGIF.ToList());
        }

        // GET: Correo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GEN_EMAILCONGIF gEN_EMAILCONGIF = db.GEN_EMAILCONGIF.Find(id);
            if (gEN_EMAILCONGIF == null)
            {
                return HttpNotFound();
            }
            return View(gEN_EMAILCONGIF);
        }

        // GET: Correo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Correo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEmailConfig,Remitente,Asunto,Cuerpo,ContrasenaEmail,SMTPHost,SMTPPort,SSL,EstatusEMail")] GEN_EMAILCONGIF gEN_EMAILCONGIF)
        {
            if (ModelState.IsValid)
            {
                db.GEN_EMAILCONGIF.Add(gEN_EMAILCONGIF);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gEN_EMAILCONGIF);
        }

        // GET: Correo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GEN_EMAILCONGIF gEN_EMAILCONGIF = db.GEN_EMAILCONGIF.Find(id);
            if (gEN_EMAILCONGIF == null)
            {
                return HttpNotFound();
            }
            return View(gEN_EMAILCONGIF);
        }

        // POST: Correo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEmailConfig,Remitente,Asunto,Cuerpo,ContrasenaEmail,SMTPHost,SMTPPort,SSL,EstatusEMail")] GEN_EMAILCONGIF gEN_EMAILCONGIF)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gEN_EMAILCONGIF).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gEN_EMAILCONGIF);
        }

        // GET: Correo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GEN_EMAILCONGIF gEN_EMAILCONGIF = db.GEN_EMAILCONGIF.Find(id);
            if (gEN_EMAILCONGIF == null)
            {
                return HttpNotFound();
            }
            return View(gEN_EMAILCONGIF);
        }

        // POST: Correo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GEN_EMAILCONGIF gEN_EMAILCONGIF = db.GEN_EMAILCONGIF.Find(id);
            db.GEN_EMAILCONGIF.Remove(gEN_EMAILCONGIF);
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
