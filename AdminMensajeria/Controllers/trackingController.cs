using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminMensajeria.Controllers
{
    public class trackingController : Controller
    {
        // GET: tracking
        public ActionResult Index()
        {
            return PartialView();
        }


       
    }
}