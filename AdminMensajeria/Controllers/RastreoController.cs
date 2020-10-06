using AdminMensajeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminMensajeria.Controllers
{
    public class RastreoController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Rastreo
        public ActionResult Index(/*string id, int ctlFolio*/)
        {
            string id = "172";
            int ctlFolio = 0;

            if (ctlFolio == 0)
            {
                int idi = Convert.ToInt32(id);
                OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(idi);
                return View("~/Views/Rastreo/Index.cshtml", oPE_SOLICITUD);

            }
            else
            {
                OPE_SOLICITUD oPE_SOLICITUD = (from c in db.OPE_SOLICITUD where c.Folio == id select c).FirstOrDefault();
                return View("~/Views/Rastreo/Index.cshtml", oPE_SOLICITUD);
            }
        }

        public JsonResult ExisteEnvio(int id)
        {
            Resultados resultado = new Resultados();

            var solicitud = (from s in db.OPE_SOLICITUD where s.IdSolicitud == id select s).ToList();

            if (solicitud.Count > 0)
                resultado.Result = true;
            else
                resultado.Result = false;

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteFolio(string id)
        {
            Resultados resultado = new Resultados();

            var solicitud = (from s in db.OPE_SOLICITUD where s.Folio == id select s).ToList();

            if (solicitud.Count > 0)
                resultado.Result = true;
            else
                resultado.Result = false;

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}