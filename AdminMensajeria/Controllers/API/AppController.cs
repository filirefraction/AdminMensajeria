using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AdminMensajeria.Clases;
using AdminMensajeria.Models;
using AdminMensajeria.Models.APP;

namespace AdminMensajeria.Controllers.API
{
    [RoutePrefix("API/app")]
    public class AppController : BaseController
    {
        [Route("Login")]
        public IHttpActionResult PostLogin(LoginApp model)
        {
            GEN_USUARIO content;
            try
            {
                content = this.db.GEN_USUARIO.Where<GEN_USUARIO>((Expression<Func<GEN_USUARIO, bool>>)(v => v.Usuario == model.user && v.Contrasena == model.password && v.TipoUsuario == 4)).FirstOrDefault<GEN_USUARIO>();
            }
            catch
            {
                return (IHttpActionResult)this.BadRequest("Datos incorrectos.");
            }
            if (content == null)
                return (IHttpActionResult)this.BadRequest("Usuario o contraseña incorrecta.");
            return (IHttpActionResult)this.Ok<GEN_USUARIO>(content);
        }

        [Route("GUIAS")]
        [ResponseType(typeof(IEnumerable<OPE_GUIA>))]
        public async Task<IHttpActionResult> PostGUIAS(Guide model)
        {
            IEnumerable<OPE_GUIA> temp;
            try
            {
                temp = (IEnumerable<OPE_GUIA>)await this.db.OPE_GUIA.Where<OPE_GUIA>((Expression<Func<OPE_GUIA, bool>>)(v => v.IdUsuario == (int?)model.UserType && DbFunctions.TruncateTime((DateTime?)v.FechaProgramadaGuia.Value) == (DateTime?)model.DateGuide.Date)).ToListAsync<OPE_GUIA>();
            }
            catch
            {
                return (IHttpActionResult)this.BadRequest("Error al recuperar Guías");
            }
            return temp != null ? (IHttpActionResult)this.Ok<IEnumerable<OPE_GUIA>>(temp) : (IHttpActionResult)this.BadRequest("Sin Datos.");
        }

        [Route("SOLICITUD/{idGuia}")]
        [ResponseType(typeof(IEnumerable<OPE_SOLICITUDPUNTOSENTREC>))]
        public async Task<IHttpActionResult> GetSOLICITUD(int idGuia)
        {
            IEnumerable<OPE_SOLICITUDPUNTOSENTREC> temp;
            try
            {
                temp = (IEnumerable<OPE_SOLICITUDPUNTOSENTREC>)await this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(v => v.IdGuia == (int?)idGuia && v.EstatusPuntosEntRec == false)).ToListAsync<OPE_SOLICITUDPUNTOSENTREC>();
                foreach (OPE_SOLICITUDPUNTOSENTREC solicitudpuntosentrec1 in temp)
                {
                    OPE_SOLICITUDPUNTOSENTREC item = solicitudpuntosentrec1;
                    OPE_SOLICITUDPUNTOSENTREC solicitudpuntosentrec = item;
                    OPE_SOLICITUD opeSolicitud = await this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.IdSolicitud == item.IdSolicitud)).FirstOrDefaultAsync<OPE_SOLICITUD>();
                    solicitudpuntosentrec.OPE_SOLICITUD = opeSolicitud;
                    solicitudpuntosentrec = (OPE_SOLICITUDPUNTOSENTREC)null;
                }
            }
            catch
            {
                return (IHttpActionResult)this.BadRequest("Error al recuperar solicitudes");
            }
            return temp != null ? (IHttpActionResult)this.Ok<IEnumerable<OPE_SOLICITUDPUNTOSENTREC>>(temp) : (IHttpActionResult)this.BadRequest("Sin Datos.");
        }

        [Route("RECIBIR/")]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> PostRECIBIR(Recibir model)
        {
            OPE_SOLICITUDPRODUCTO producto;
            OPE_SOLICITUD solicitud;
            try
            {
                producto = await this.db.OPE_SOLICITUDPRODUCTO.Where<OPE_SOLICITUDPRODUCTO>((Expression<Func<OPE_SOLICITUDPRODUCTO, bool>>)(x => x.IdSolicitud == model.id)).FirstOrDefaultAsync<OPE_SOLICITUDPRODUCTO>();
                solicitud = await this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.IdSolicitud == model.id)).FirstOrDefaultAsync<OPE_SOLICITUD>();
            }
            catch
            {
                return (IHttpActionResult)this.BadRequest("Error.");
            }
            if (producto == null)
                return (IHttpActionResult)this.NotFound();
            if (producto.Recibido)
                return (IHttpActionResult)this.Unauthorized();
            producto.Recibido = true;
            producto.FechaRecepcion = new DateTime?(helper.dateTimeZone(DateTime.Now));
            producto.Receptor = model.name;
            solicitud.EstatusSolicitud = 2;
            try
            {
                this.db.Entry<OPE_SOLICITUDPRODUCTO>(producto).State = EntityState.Modified;
                this.db.Entry<OPE_SOLICITUD>(solicitud).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                return (IHttpActionResult)this.BadRequest("Error.");
            }
            return (IHttpActionResult)this.Ok<string>("Éxito.");
        }

        [Route("UPDATE/")]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> PostUPDATE(SolicitudUpdate model)
        {
            List<OPE_SOLICITUDPUNTOSENTREC> entrec = new List<OPE_SOLICITUDPUNTOSENTREC>();
            List<OPE_SOLICITUD> solicitudes = new List<OPE_SOLICITUD>();
            try
            {
                foreach (SolicitudApp solicitude in model.solicitudes)
                {
                    SolicitudApp item = solicitude;
                    OPE_SOLICITUDPUNTOSENTREC solicitudpuntosentrec = await this.db.OPE_SOLICITUDPUNTOSENTREC.Where<OPE_SOLICITUDPUNTOSENTREC>((Expression<Func<OPE_SOLICITUDPUNTOSENTREC, bool>>)(x => x.IdSolicitud == item.IdSolicitud && x.TipoPunto == 2)).FirstAsync<OPE_SOLICITUDPUNTOSENTREC>();
                    entrec.Add(solicitudpuntosentrec);
                }
                foreach (SolicitudApp solicitude in model.solicitudes)
                {
                    SolicitudApp item = solicitude;
                    OPE_SOLICITUD opeSolicitud = await this.db.OPE_SOLICITUD.Where<OPE_SOLICITUD>((Expression<Func<OPE_SOLICITUD, bool>>)(x => x.IdSolicitud == item.IdSolicitud)).FirstAsync<OPE_SOLICITUD>();
                    solicitudes.Add(opeSolicitud);
                }
            }
            catch
            {
                return (IHttpActionResult)this.BadRequest("Error.");
            }
            if (entrec.Count <= 0)
                return (IHttpActionResult)this.NotFound();
            if (!model.IsComplaint)
            {
                foreach (OPE_SOLICITUDPUNTOSENTREC entity in entrec)
                {
                    entity.NombrePuntosEntRec = model.Receptor;
                    entity.FotoPuntosEntRec = model.Img;
                    entity.FirmaPuntosEntRec = model.Sign;
                    entity.FechaReal = new DateTime?(helper.dateTimeZone(DateTime.Now));
                    entity.Latitud = model.Latitud;
                    entity.Longitud = model.Longitud;
                    entity.EstatusPuntosEntRec = true;
                    this.db.Entry<OPE_SOLICITUDPUNTOSENTREC>(entity).State = EntityState.Modified;
                }
                foreach (OPE_SOLICITUD entity in solicitudes)
                {
                    entity.EstatusSolicitud = 4;
                    this.db.Entry<OPE_SOLICITUD>(entity).State = EntityState.Modified;
                }
                try
                {
                    this.db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return (IHttpActionResult)this.BadRequest("Error.");
                }
            }
            else
            {
                foreach (OPE_SOLICITUDPUNTOSENTREC entity in entrec)
                {
                    entity.Problema = model.IsComplaint;
                    entity.ObsProblema = model.Problema;
                    entity.FechaReal = new DateTime?(helper.dateTimeZone(DateTime.Now));
                    this.db.Entry<OPE_SOLICITUDPUNTOSENTREC>(entity).State = EntityState.Modified;
                }
                foreach (OPE_SOLICITUD entity in solicitudes)
                {
                    entity.EstatusSolicitud = 4;
                    this.db.Entry<OPE_SOLICITUD>(entity).State = EntityState.Modified;
                }
                try
                {
                    this.db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return (IHttpActionResult)this.BadRequest("Error.");
                }
            }
            return (IHttpActionResult)this.Ok<string>("Éxito.");
        }
    }
}
