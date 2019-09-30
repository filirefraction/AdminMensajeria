using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminMensajeria.Models;
using System.Text;
using System.IO;

namespace AdminMensajeria.Controllers
{
    public class ReporteController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Reporte
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reporte/Details/5
        public ActionResult Details(DateTime Desde, DateTime Hasta)
        {
            Hasta = Hasta.AddDays(1);

            int IdUsuario;
            if (Session["IdUsuario"] == null)
            {
                ViewBag.idusuario = 0;
                ViewBag.tipousuario = 0;
                ViewBag.desde = Desde;
                ViewBag.hasta = Hasta;
            }
            else
            {
                IdUsuario = (int)Session["IdUsuario"];
                ViewBag.idusuario = IdUsuario;
                ViewBag.tipousuario = (from u in db.GEN_USUARIO where u.IdUsuario == IdUsuario select u.TipoUsuario).FirstOrDefault();
                ViewBag.desde = Desde;
                ViewBag.hasta = Hasta;
            }
            return PartialView("~/Views/Reporte/Details.cshtml");
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
