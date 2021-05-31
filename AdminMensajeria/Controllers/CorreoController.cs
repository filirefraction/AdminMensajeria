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

        public ActionResult Create() => (ActionResult)this.View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEmailConfig,Remitente,Asunto,Cuerpo,ContrasenaEmail,SMTPHost,SMTPPort,SSL,EstatusEMail")] GEN_EMAILCONGIF gEN_EMAILCONGIF)
        {
            if (!this.ModelState.IsValid)
                return (ActionResult)this.View((object)gEN_EMAILCONGIF);
            this.db.GEN_EMAILCONGIF.Add(gEN_EMAILCONGIF);
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GEN_EMAILCONGIF genEmailcongif = this.db.GEN_EMAILCONGIF.Find(new object[1]
            {
        (object) id
            });
            return genEmailcongif == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)genEmailcongif);
        }

        public JsonResult EditEmail(GEN_EMAILCONGIF Email)
        {
            Resultados resultados = new Resultados();
            try
            {
                this.db.Entry<GEN_EMAILCONGIF>(Email).State = EntityState.Modified;
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

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GEN_EMAILCONGIF genEmailcongif = this.db.GEN_EMAILCONGIF.Find(new object[1]
            {
        (object) id
            });
            return genEmailcongif == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)genEmailcongif);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.db.GEN_EMAILCONGIF.Remove(this.db.GEN_EMAILCONGIF.Find(new object[1]
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
