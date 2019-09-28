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
    public class PuntosController : ApiController
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: api/Puntos
        public IQueryable<PuntosDTO> GetPuntos()
        {
            //return db.Puntos;

            var Puntos = from o in db.OPE_SOLICITUDPUNTOSENTREC
                           .Where(o => o.TipoPunto == 2)
                         select new PuntosDTO()
                         {
                             Id = o.IdSolicitudPuntosEntRec,
                             IdSolicitud = o.IdSolicitud,
                             FirmaPuntosEntRec = o.FirmaPuntosEntRec,
                             FotoPuntosEntRec = o.FotoPuntosEntRec,
                             Latitud = o.Latitud,
                             Longitud = o.Longitud,
                             Problema = o.Problema,
                             ObsProblema = o.ObsProblema,
                             Estatus = o.EstatusPuntosEntRec
                         };

            return Puntos;
        }

        // GET: api/Puntos/5
        [ResponseType(typeof(PuntosDTO))]
        public IHttpActionResult GetPuntos(int id)
        {
            var PuntosDTO = from o in db.OPE_SOLICITUDPUNTOSENTREC
                             .Where(o => o.IdSolicitudPuntosEntRec == id)
                             .OrderBy(o => o.IdSolicitudPuntosEntRec)
                             .Select(o => new PuntosDTO()
                             {
                                 Id = o.IdSolicitudPuntosEntRec,
                                 IdSolicitud = o.IdSolicitud,
                                 FirmaPuntosEntRec = o.FirmaPuntosEntRec,
                                 FotoPuntosEntRec = o.FotoPuntosEntRec,
                                 Latitud = o.Latitud,
                                 Longitud = o.Longitud,
                                 Problema = o.Problema,
                                 ObsProblema = o.ObsProblema,
                                 Estatus = o.EstatusPuntosEntRec
                             })
                            select o;

            if (PuntosDTO == null)
            {
                return NotFound();
            }

            return Ok(PuntosDTO);

            //Puntos Puntos = db.Puntos.Find(id);
            //if (Puntos == null)
            //{
            //    return NotFound();
            //}

            // return Ok(Puntos);
        }

        // PUT: api/Puntos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPuntos(int id, OPE_SOLICITUDPUNTOSENTREC Puntos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Puntos.IdSolicitudPuntosEntRec)
            {
                return BadRequest();
            }

            db.Entry(Puntos).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuntosExists(id))
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

        // POST: api/Puntos
        [ResponseType(typeof(OPE_SOLICITUDPUNTOSENTREC))]
        public IHttpActionResult PostPuntos(OPE_SOLICITUDPUNTOSENTREC Puntos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OPE_SOLICITUDPUNTOSENTREC.Add(Puntos);

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Puntos.IdSolicitudPuntosEntRec }, Puntos);
        }

        //// POST: api/Puntos
        //[ResponseType(typeof(Puntos))]
        //public IHttpActionResult PostPuntos(Puntos Puntos)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Puntos.Add(Puntos);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = Puntos.IdOrden }, Puntos);
        //}

        // DELETE: api/Puntos/5
        [ResponseType(typeof(OPE_SOLICITUDPUNTOSENTREC))]
        public IHttpActionResult DeletePuntos(int id)
        {
            OPE_SOLICITUDPUNTOSENTREC Puntos = db.OPE_SOLICITUDPUNTOSENTREC.Find(id);
            if (Puntos == null)
            {
                return NotFound();
            }

            db.OPE_SOLICITUDPUNTOSENTREC.Remove(Puntos);
            db.SaveChanges();

            return Ok(Puntos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PuntosExists(int id)
        {
            return db.OPE_SOLICITUDPUNTOSENTREC.Count(e => e.IdSolicitudPuntosEntRec == id) > 0;
        }
    }
}