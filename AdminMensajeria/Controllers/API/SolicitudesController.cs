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
    public class SolicitudesController : ApiController
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: api/Solicitudes
        public IQueryable<SolicitudesDTO> GetSolicitudes()
        {
            //return db.Solicitudes;

            var Solicitudes = from o in db.OPE_SOLICITUD
                              where o.EstatusSolicitud == 3
                              select new SolicitudesDTO()
                         {
                             Id = o.IdSolicitud,
                             Estatus = o.EstatusSolicitud
                         };

            return Solicitudes;
        }

        // GET: api/Solicitudes/5
        [ResponseType(typeof(SolicitudesDTO))]
        public IHttpActionResult GetSolicitudes(int id)
        {
            var SolicitudesDTO = from o in db.OPE_SOLICITUD
                             .Where(o => o.IdSolicitud== id)
                             .OrderBy(o => o.IdSolicitud)
                             .Select(o => new SolicitudesDTO()
                             {
                                 Id = o.IdSolicitud,
                                 Estatus = o.EstatusSolicitud
                             })
                            select o;

            if (SolicitudesDTO == null)
            {
                return NotFound();
            }

            return Ok(SolicitudesDTO);

            //Solicitudes Solicitudes = db.Solicitudes.Find(id);
            //if (Solicitudes == null)
            //{
            //    return NotFound();
            //}

            // return Ok(Solicitudes);
        }

        // PUT: api/Solicitudes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSolicitudes(int id, OPE_SOLICITUD Solicitudes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Solicitudes.IdSolicitud)
            {
                return BadRequest();
            }

            db.Entry(Solicitudes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudesExists(id))
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

        // POST: api/Solicitudes
        [ResponseType(typeof(OPE_SOLICITUD))]
        public IHttpActionResult PostSolicitudes(OPE_SOLICITUD solicitudes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OPE_SOLICITUD.Add(solicitudes);

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = solicitudes.IdSolicitud }, solicitudes);
        }

        //// POST: api/Solicitudes
        //[ResponseType(typeof(Solicitudes))]
        //public IHttpActionResult PostSolicitudes(Solicitudes Solicitudes)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Solicitudes.Add(Solicitudes);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = Solicitudes.IdOrden }, Solicitudes);
        //}

        // DELETE: api/Solicitudes/5
        [ResponseType(typeof(OPE_SOLICITUD))]
        public IHttpActionResult DeleteSolicitudes(int id)
        {
            OPE_SOLICITUD Solicitudes = db.OPE_SOLICITUD.Find(id);
            if (Solicitudes == null)
            {
                return NotFound();
            }

            db.OPE_SOLICITUD.Remove(Solicitudes);
            db.SaveChanges();

            return Ok(Solicitudes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SolicitudesExists(int id)
        {
            return db.OPE_SOLICITUD.Count(e => e.IdSolicitud == id) > 0;
        }
    }
}