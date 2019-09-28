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

namespace AdminMensajeria.Controllers.API
{
    [RoutePrefix("API/Usuarios")]
    public class UsuariosController : ApiController
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Longin(JObject form)
        {
            var usuario = string.Empty;
            dynamic jsonObject = form;

            try
            {
                usuario = jsonObject.Usuario.Value;
            }
            catch
            {
                return this.BadRequest("Datos incorrectos.");
            }

            var usu = db.GEN_USUARIO
                .Where(v => v.Usuario == usuario)
                .FirstOrDefault();

            if (usu == null)
            {
                return this.BadRequest("Usuario o contraseña incorrecta.");
            }

            return this.Ok(usu);

        }

        // GET: api/Usuarios
        public IQueryable<UsuariosDTO> GetUsuarios()
        {
            var usuarios = from c in db.GEN_USUARIO
                            .Where(o => o.TipoUsuario == 4 && o.EstatusUsuario == true)
                           select new UsuariosDTO()
                          {
                              Id= c.IdUsuario,
                              TipoUsuario = c.TipoUsuario,
                              Usuario = c.Usuario,
                              Contrasena = c.Contrasena,
                              Estatus= c.EstatusUsuario
                          };

            return usuarios;
        }

        // GET: api/Usuarios/5
        [ResponseType(typeof(GEN_USUARIO))]
        public IHttpActionResult GetUsuarios(int id)
        {
            GEN_USUARIO usuarios = db.GEN_USUARIO.Find(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return Ok(usuarios);
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuarios(int id, GEN_USUARIO usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuarios.IdUsuario)
            {
                return BadRequest();
            }

            db.Entry(usuarios).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Usuarios
        [ResponseType(typeof(GEN_USUARIO))]
        public IHttpActionResult PostUsuarios(GEN_USUARIO usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GEN_USUARIO.Add(usuarios);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = usuarios.IdUsuario }, usuarios);
        }

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(GEN_USUARIO))]
        public IHttpActionResult DeleteUsuarios(int id)
        {
            GEN_USUARIO usuarios = db.GEN_USUARIO.Find(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            db.GEN_USUARIO.Remove(usuarios);
            db.SaveChanges();

            return Ok(usuarios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuariosExists(int id)
        {
            return db.GEN_USUARIO.Count(e => e.IdUsuario == id) > 0;
        }
    }
}