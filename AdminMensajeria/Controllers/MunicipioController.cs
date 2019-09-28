using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminMensajeria.Models;

namespace AdminMensajeria.Controllers
{
    public class MunicipioController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Municipio
        public ActionResult Index()
        {
            ViewBag.ListPais = new SelectList(db.GEN_PAIS, "IdPais", "Pais");
            return View();
        }

        // GET: Municpios
        public ActionResult Municipios(int id)
        {
            var gEN_MUNICIPIO = db.GEN_MUNICIPIO.Where(x => x.IdEstado == id);
            return PartialView(gEN_MUNICIPIO.ToList());
        }

        // GET: Municipio/Create
        public ActionResult Create(int id)
        {
            int idpais = (from p in db.GEN_ESTADO where p.IdEstado == id select p.IdPais).FirstOrDefault();
            ViewBag.estado = id;
            ViewBag.pais = idpais;

            ViewBag.ListPais = new SelectList(db.GEN_PAIS, "IdPais", "Pais");
            ViewBag.ListEstado = new SelectList(db.GEN_ESTADO.Where(x => x.IdPais == idpais), "IdEstado", "CiudadEstado");
            return PartialView("~/Views/Municipio/Create.cshtml");
        }

        public JsonResult CreateMunicipio(GEN_MUNICIPIO gEN_Municipio)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.GEN_MUNICIPIO.Add(gEN_Municipio);
                db.SaveChanges();
                resultado.Result = true;
            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
                resultado.Valor = 0;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        // GET: Municipio/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.idmunicipio = id; 

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GEN_MUNICIPIO gEN_MUNICIPIO = db.GEN_MUNICIPIO.Find(id);
            if (gEN_MUNICIPIO == null)
            {
                return HttpNotFound();
            }

            int idestado = gEN_MUNICIPIO.IdEstado;
            int idpais = (from p in db.GEN_ESTADO where p.IdEstado == idestado select p.IdPais).FirstOrDefault();
            ViewBag.estado = idestado;
            ViewBag.pais = idpais;

            ViewBag.ListPais = new SelectList(db.GEN_PAIS, "IdPais", "Pais");
            ViewBag.ListEstado = new SelectList(db.GEN_ESTADO.Where(x => x.IdPais == idpais), "IdEstado", "CiudadEstado");
            return PartialView("~/Views/Municipio/Edit.cshtml", gEN_MUNICIPIO);
        }

        public JsonResult EditMunicipio(GEN_MUNICIPIO gEN_Municipio)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(gEN_Municipio).State = EntityState.Modified;
                db.SaveChanges();
                resultado.Result = true;
            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
                resultado.Valor = 0;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaEstados(int id)
        {
            var Estados = (from e in db.GEN_ESTADO
                           where e.IdPais == id
                           select new
                           {
                               idestado = e.IdEstado,
                               estado = e.CiudadEstado
                           }).ToList();

            return Json(Estados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMunicipio(int id)
        {
            Resultados results = new Resultados();
            GEN_MUNICIPIO Municipio = db.GEN_MUNICIPIO.Find(id);

            try
            {
                db.GEN_MUNICIPIO.Remove(Municipio);
                db.SaveChanges();
                results.Result = true;
                results.Mensaje = "Municipio Eliminado Correctamente";
            }
            catch (Exception ex)
            {
                results.Result = false;
                results.Mensaje = ex.Message;
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
