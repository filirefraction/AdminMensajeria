using AdminMensajeria.Models;

using System.Web.Http;


namespace AdminMensajeria.Controllers.API
{
    public class BaseController : ApiController
    {
        protected MensajeriaEntities db;
        public BaseController()
        {
            db = new MensajeriaEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
        }

    }
}
