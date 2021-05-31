using AdminMensajeria.Models;
using System.Web.Mvc;

namespace AdminMensajeria.Controllers
{
    public class QrViewController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        public ActionResult Index(int id) => (ActionResult)this.View((object)this.db.OPE_SOLICITUD.Find(id));
    }
}
