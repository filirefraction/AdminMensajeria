using AdminMensajeria.Models;
using System.Web.Http;

namespace AdminMensajeria.Controllers.API
{
    public class BaseController : ApiController
    {
        protected MensajeriaEntities db;

        public BaseController()
        {
            this.db = new MensajeriaEntities();
            this.db.Configuration.LazyLoadingEnabled = false;
            this.db.Configuration.ProxyCreationEnabled = false;
        }
    }
}