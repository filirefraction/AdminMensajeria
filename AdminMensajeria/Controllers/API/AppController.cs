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

        [Route("SOLICITUD/{idGuia}")]
        [ResponseType(typeof(IEnumerable<OPE_SOLICITUDPUNTOSENTREC>))]
        public async Task<IHttpActionResult> GetSOLICITUD(int idGuia)
        {
            IEnumerable<OPE_SOLICITUDPUNTOSENTREC> temp;

            try
            {
                temp = await db.OPE_SOLICITUDPUNTOSENTREC
                .Where(v => v.IdGuia == idGuia && v.EstatusPuntosEntRec == false).ToListAsync();
            }
            catch
            {
                return BadRequest("Error al recuperar solicitudes");
            }

            if (temp == null)
            {
                return BadRequest("Sin Datos.");
            }

            return Ok(temp);

        }

        [Route("RECIBIR/")]
        [ResponseType(typeof(String))]
        public async Task<IHttpActionResult> PostRECIBIR(Recibir model)
        {
            OPE_SOLICITUDPRODUCTO producto;
            OPE_SOLICITUD solicitud;

            try
            {
                producto = await db.OPE_SOLICITUDPRODUCTO.Where(x => x.IdSolicitud == model.id).FirstOrDefaultAsync();
                solicitud = await db.OPE_SOLICITUD.Where(x => x.IdSolicitud == model.id).FirstOrDefaultAsync();
            }
            catch
            {
                return BadRequest("Error.");
            }

            if (producto != null)
            {
                if (producto.Recibido != true)
                {
                    producto.Recibido = true;
                    producto.FechaRecepcion = DateTime.Now.AddHours(1); //Producción
                    producto.Receptor = model.name;
                    solicitud.EstatusSolicitud = 2;

                    try
                    {
                        db.Entry(producto).State = EntityState.Modified;
                        db.Entry(solicitud).State = EntityState.Modified;
                        db.SaveChanges();
                        
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Error.");
                    }
                }
                else
                {
                    return Unauthorized();
                }

            }
            else
            {
                return NotFound();
            }

            return Ok("Éxito.");
        }

        [Route("UPDATE/")]
        [ResponseType(typeof(String))]
        public async Task<IHttpActionResult> PostUPDATE(SolicitudUpdate model)
        {
            List<OPE_SOLICITUDPUNTOSENTREC> entrec = new List<OPE_SOLICITUDPUNTOSENTREC>();
            List<OPE_SOLICITUD> solicitudes = new List<OPE_SOLICITUD>();


            try
            {
                //entrec = await db.OPE_SOLICITUDPUNTOSENTREC.Where(x => x.IdGuia == model.IdGuia).ToListAsync();
                foreach (SolicitudApp item in model.solicitudes)
                {
                    OPE_SOLICITUDPUNTOSENTREC temp = await db.OPE_SOLICITUDPUNTOSENTREC.Where(x => x.IdSolicitud == item.IdSolicitud && x.TipoPunto == 2).FirstAsync();
                    entrec.Add(temp);
                }

                foreach (SolicitudApp item in model.solicitudes)
                {
                    OPE_SOLICITUD temp = await db.OPE_SOLICITUD.Where(x => x.IdSolicitud == item.IdSolicitud).FirstAsync();
                    solicitudes.Add(temp);
                }

            }
            catch
            {
                return BadRequest("Error.");
            }

            if (entrec.Count > 0)
            {
                if (!model.IsComplaint)
                {
                    foreach (OPE_SOLICITUDPUNTOSENTREC item in entrec)
                    {
                        item.NombrePuntosEntRec = model.Receptor;
                        item.FotoPuntosEntRec = model.Img;
                        item.FirmaPuntosEntRec = model.Sign;
                        item.FechaReal = DateTime.Now.AddHours(1);
                        item.Latitud = model.Latitud;
                        item.Longitud = model.Longitud;
                        db.Entry(item).State = EntityState.Modified;
                    }

                    foreach (OPE_SOLICITUD item in solicitudes)
                    {
                        item.EstatusSolicitud = 4;
                        db.Entry(item).State = EntityState.Modified;

                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Error.");
                    }
                }
                else
                {
                    foreach (OPE_SOLICITUDPUNTOSENTREC item in entrec)
                    {
                        item.Problema = model.IsComplaint;
                        item.ObsProblema = model.Problema;
                        item.FechaReal = DateTime.Now.AddHours(1);
                        db.Entry(item).State = EntityState.Modified;
                    }

                    foreach (OPE_SOLICITUD item in solicitudes)
                    {
                        item.EstatusSolicitud = 4;
                        db.Entry(item).State = EntityState.Modified;

                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Error.");
                    }
                }
            }
            else
            {
                return NotFound();
            }

            return Ok("Éxito.");
        }

    }
}
