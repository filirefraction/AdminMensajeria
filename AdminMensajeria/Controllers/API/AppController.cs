using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AdminMensajeria.Models;
using Newtonsoft.Json.Linq;
using AdminMensajeria.Models.DTOs;
using AdminMensajeria.Models.APP;
using System.Threading.Tasks;

namespace AdminMensajeria.Controllers.API
{
    [RoutePrefix("API/app")]
    public class AppController : BaseController
    {
        [Route("Login")]
        public IHttpActionResult PostLogin(LoginApp model)
        {
            GEN_USUARIO temp;

            try
            {
                temp = db.GEN_USUARIO
                .Where(v => v.Usuario == model.user && v.Contrasena == model.password && v.TipoUsuario == 4)
                .FirstOrDefault();
            }
            catch
            {
                return this.BadRequest("Datos incorrectos.");
            }

            if (temp == null)
            {
                return this.BadRequest("Usuario o contraseña incorrecta.");
            }

            return this.Ok(temp);

        }

        [Route("GUIAS")]
        [ResponseType(typeof(IEnumerable<OPE_GUIA>))]
        public async Task<IHttpActionResult> PostGUIAS(Guide model)
        {
            IEnumerable<OPE_GUIA> temp;

            try
            {
                temp = await db.OPE_GUIA
                .Where(v => v.IdUsuario == model.UserType && DbFunctions.TruncateTime(v.FechaProgramadaGuia.Value) == model.DateGuide.Date).ToListAsync();               
            }
            catch
            {
                return BadRequest("Error al recuperar Guías");
            }

            if (temp == null)
            {
                return BadRequest("Sin Datos.");
            }

            return Ok(temp);

        }

    }
}
