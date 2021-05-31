using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminMensajeria.Models;

namespace AdminMensajeria.Controllers
{
    public class ColoniaController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Colonia
        public ActionResult Index()
        {
            ViewBag.ListPais = new SelectList(db.GEN_PAIS, "IdPais", "Pais");
            return View();
        }

        // GET: Colonias
        public ActionResult Colonias(int id)
        {
            var gEN_COLONIA = db.GEN_COLONIA.Where(x => x.IdMunicipio == id);
            return PartialView(gEN_COLONIA.ToList());
        }

        // GET: Colonia/Create
        public ActionResult Create(int id)//recibe ID Municipio
        {
            int IdEstado = (from m in db.GEN_MUNICIPIO where m.IdMunicipio == id select m.IdEstado).FirstOrDefault();
            int IdPais = (from p in db.GEN_ESTADO where p.IdEstado == IdEstado select p.IdPais).FirstOrDefault();
            ViewBag.municipio = id;
            ViewBag.estado = IdEstado;
            ViewBag.pais = IdPais;

            ViewBag.ListMunicipio = new SelectList(db.GEN_MUNICIPIO.Where(x => x.IdEstado == IdEstado), "IdMunicipio", "Municipio");
            ViewBag.ListEstado = new SelectList(db.GEN_ESTADO.Where(x => x.IdPais == IdPais), "IdEstado", "CiudadEstado");
            ViewBag.ListPais = new SelectList(db.GEN_PAIS, "IdPais", "Pais");

            return PartialView("~/Views/Colonia/Create.cshtml");
        }

        public JsonResult CreateColonia(GEN_COLONIA gEN_Colonia)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.GEN_COLONIA.Add(gEN_Colonia);
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

        // GET: Colonia/Edit/5
        public ActionResult Edit(int? id)
        {
           GEN_COLONIA gEN_COLONIA = db.GEN_COLONIA.Find(id);

            ViewBag.idcolonia = id;
            ViewBag.colonia = gEN_COLONIA.Colonia;
            ViewBag.idcodigop = gEN_COLONIA.IdCodigoP;
            ViewBag.codigopostal = gEN_COLONIA.GEN_CODIGOPOSTAL.CodigoPostal;

            int IdMunicipio = gEN_COLONIA.IdMunicipio;
            int IdEstado = (from m in db.GEN_MUNICIPIO where m.IdMunicipio == IdMunicipio select m.IdEstado).FirstOrDefault();
            int IdPais = (from p in db.GEN_ESTADO where p.IdEstado == IdEstado select p.IdPais).FirstOrDefault();
            ViewBag.municipio = IdMunicipio;
            ViewBag.estado = IdEstado;
            ViewBag.pais = IdPais;

            ViewBag.ListMunicipio = new SelectList(db.GEN_MUNICIPIO.Where(x => x.IdEstado == IdEstado), "IdMunicipio", "Municipio");
            ViewBag.ListEstado = new SelectList(db.GEN_ESTADO.Where(x => x.IdPais == IdPais), "IdEstado", "CiudadEstado");
            ViewBag.ListPais = new SelectList(db.GEN_PAIS, "IdPais", "Pais");

            return PartialView("~/Views/Colonia/Edit.cshtml");
        }

        public JsonResult EditColonia(GEN_COLONIA gEN_Colonia)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(gEN_Colonia).State = EntityState.Modified;
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
        
        public JsonResult ListaMunicipios(int id)
        {
            var Municipios = (from m in db.GEN_MUNICIPIO
                           where m.IdEstado == id
                           select new
                           {
                               idmunicipio = m.IdMunicipio,
                               municipio = m.Municipio
                           }).ToList();

            return Json(Municipios, JsonRequestBehavior.AllowGet);
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

        public JsonResult ValidarCodigoPostal(string id)
        {
            Resultados resultados = new Resultados();
            int num = this.db.GEN_CODIGOPOSTAL.Where<GEN_CODIGOPOSTAL>((Expression<Func<GEN_CODIGOPOSTAL, bool>>)(c => c.CodigoPostal == id)).Select<GEN_CODIGOPOSTAL, int>((Expression<Func<GEN_CODIGOPOSTAL, int>>)(c => c.IdCodigoP)).FirstOrDefault<int>();
            if (num != 0)
            {
                resultados.Result = true;
                resultados.Valor = num;
                resultados.Mensaje = "Código Postal Verificado";
            }
            else
            {
                resultados.Result = false;
                resultados.Mensaje = "¡Importante!, Este Código Postal No Esta Registrado, ¿Desea Registrarlo?";
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegistrarCodigoPostal(string id)
        {
            Resultados resultado = new Resultados();
            GEN_CODIGOPOSTAL CodigoPostal = new GEN_CODIGOPOSTAL();
            CodigoPostal.CodigoPostal = id;

            try
            {
                db.GEN_CODIGOPOSTAL.Add(CodigoPostal);
                db.SaveChanges();
                resultado.Result = true;
                resultado.Valor = (from c in db.GEN_CODIGOPOSTAL where c.CodigoPostal == id select c.IdCodigoP).FirstOrDefault();
                resultado.Mensaje = "Código Postal Verificado";

            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
                resultado.Valor = 0;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteColonia(int id)
        {
            Resultados results = new Resultados();
            GEN_COLONIA Colonia = db.GEN_COLONIA.Find(id);

            try
            {
                db.GEN_COLONIA.Remove(Colonia);
                db.SaveChanges();
                results.Result = true;
                results.Mensaje = "Colonia Eliminado Correctamente";
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
