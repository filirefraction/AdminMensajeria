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

        public ActionResult Index() => (ActionResult)this.View((object)this.db.GEN_UNIDAD.ToList<GEN_UNIDAD>());

        public ActionResult Create() => (ActionResult)this.PartialView("~/Views/Unidad/Create.cshtml");

        public JsonResult CreateUnidad(GEN_UNIDAD Unidad)
        {
            Resultados resultados = new Resultados();
            try
            {
                this.db.GEN_UNIDAD.Add(Unidad);
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

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GEN_UNIDAD genUnidad = this.db.GEN_UNIDAD.Find(new object[1]
            {
        (object) id
            });
            return genUnidad == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.PartialView("~/Views/Unidad/Edit.cshtml", (object)genUnidad);
        }

        public JsonResult EditUnidad(GEN_UNIDAD Unidad)
        {
            Resultados resultados = new Resultados();
            try
            {
                this.db.Entry<GEN_UNIDAD>(Unidad).State = EntityState.Modified;
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

        public JsonResult DeleteUnidad(int id)
        {
            Resultados resultados = new Resultados();
            GEN_UNIDAD entity = this.db.GEN_UNIDAD.Find(new object[1]
            {
        (object) id
            });
            try
            {
                this.db.GEN_UNIDAD.Remove(entity);
                this.db.SaveChanges();
                resultados.Result = true;
                resultados.Mensaje = "País Eliminado Correctamente";
            }
            catch (Exception ex)
            {
                resultados.Result = false;
                resultados.Mensaje = ex.Message;
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
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
