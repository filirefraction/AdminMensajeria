using System.Web.Mvc;

namespace AdminMensajeria.Controllers
{
    public class trackingController : Controller
    {
        
            public ActionResult Index() => (ActionResult)this.PartialView();

    }
}