using AdminMensajeria.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;


namespace AdminMensajeria.Controllers
{
    public class RastreoController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();
        // GET: Rastreo
        public ActionResult Index()
        {
            string id = "172";
            if (true)
                return (ActionResult)this.View("~/Views/Rastreo/Index.cshtml", (object)this.db.OPE_SOLICITUD.Find(new object[1]
                {
          (object) Convert.ToInt32(id)
                }));
            return (ActionResult)this.View("~/Views/Rastreo/Index.cshtml", (object)this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(c => c.Folio == id)).FirstOrDefault<OPE_SOLICITUD>());
        }

        public JsonResult ExisteEnvio(int id)
        {
            Resultados resultados = new Resultados();
            DbSet<OPE_SOLICITUD> opeSolicitud = this.db.OPE_SOLICITUD;
            Expression<Func<OPE_SOLICITUD, bool>> predicate = (Expression<Func<OPE_SOLICITUD, bool>>)(s => s.IdSolicitud == id);
            resultados.Result = opeSolicitud.Where<OPE_SOLICITUD>(predicate).ToList<OPE_SOLICITUD>().Count > 0;
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteFolio(string id)
        {
            Resultados resultados = new Resultados();
            DbSet<OPE_SOLICITUD> opeSolicitud = this.db.OPE_SOLICITUD;
            Expression<Func<OPE_SOLICITUD, bool>> predicate = (Expression<Func<OPE_SOLICITUD, bool>>)(s => s.Folio == id);
            resultados.Result = opeSolicitud.Where<OPE_SOLICITUD>(predicate).ToList<OPE_SOLICITUD>().Count > 0;
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }
    }
}