using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdminMensajeria.Startup))]
namespace AdminMensajeria
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
