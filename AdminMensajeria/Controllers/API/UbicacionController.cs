using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AdminMensajeria.Models;

namespace AdminMensajeria.Controllers.API
{
    public class UbicacionController : ApiController
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: api/Ubicacion
        public IQueryable<BET_UBICACION> GetBET_UBICACION()
        {
            return db.BET_UBICACION;
        }

        // GET: api/Ubicacion/5
        [ResponseType(typeof(BET_UBICACION))]
        public async Task<IHttpActionResult> GetBET_UBICACION(int id)
        {
            BET_UBICACION bET_UBICACION = await db.BET_UBICACION.FindAsync(id);
            if (bET_UBICACION == null)
            {
                return NotFound();
            }

            return Ok(bET_UBICACION);
        }

        // PUT: api/Ubicacion/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBET_UBICACION(int id, BET_UBICACION bET_UBICACION)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bET_UBICACION.ID)
            {
                return BadRequest();
            }

            db.Entry(bET_UBICACION).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BET_UBICACIONExists(id))
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

        // POST: api/Ubicacion
        [ResponseType(typeof(BET_UBICACION))]
        public async Task<IHttpActionResult> PostBET_UBICACION(BET_UBICACION bET_UBICACION)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BET_UBICACION.Add(bET_UBICACION);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = bET_UBICACION.ID }, bET_UBICACION);
        }

        // DELETE: api/Ubicacion/5
        [ResponseType(typeof(BET_UBICACION))]
        public async Task<IHttpActionResult> DeleteBET_UBICACION(int id)
        {
            BET_UBICACION bET_UBICACION = await db.BET_UBICACION.FindAsync(id);
            if (bET_UBICACION == null)
            {
                return NotFound();
            }

            db.BET_UBICACION.Remove(bET_UBICACION);
            await db.SaveChangesAsync();

            return Ok(bET_UBICACION);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BET_UBICACIONExists(int id)
        {
            return db.BET_UBICACION.Count(e => e.ID == id) > 0;
        }
    }
}