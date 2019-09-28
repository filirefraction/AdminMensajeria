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
    public class ClienteController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Cliente
        public ActionResult Index()
        {
            ViewBag.ListCategoria = new SelectList(db.OPE_CATEGORIA, "IdCategoria", "Categoria");
            return View();
        }

        public ActionResult Clientes(int id)
        {
            var oPE_CLIENTES = db.OPE_CLIENTPROVEE.Where(x => x.IdCategoria == id);
            return PartialView(oPE_CLIENTES.ToList());
        }

        // GET: Cliente/Create
        public ActionResult Create(int id)
        {
            ViewBag.IdCategoria = new SelectList(db.OPE_CATEGORIA, "IdCategoria", "Categoria", id);
            return PartialView("~/Views/Cliente/Create.cshtml");
        }

        public JsonResult CreateCliente(OPE_CLIENTPROVEE oPE_Cliente)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.OPE_CLIENTPROVEE.Add(oPE_Cliente);
                db.SaveChanges();
                resultado.Result = true;
                resultado.Valor = (from a in db.OPE_CLIENTPROVEE orderby a.IdClientProvee descending select a.IdClientProvee).First();
            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
                resultado.Valor = 0;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_CLIENTPROVEE oPE_CLIENTPROVEE = db.OPE_CLIENTPROVEE.Find(id);
            if (oPE_CLIENTPROVEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategoria = new SelectList(db.OPE_CATEGORIA, "IdCategoria", "Categoria", oPE_CLIENTPROVEE.IdCategoria);
            return View(oPE_CLIENTPROVEE);
        }

        public JsonResult EditCliente(OPE_CLIENTPROVEE oPE_CLIENTE)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(oPE_CLIENTE).State = EntityState.Modified;
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

        public JsonResult DeleteCliente(int id)
        {
            Resultados results = new Resultados();
            OPE_CLIENTPROVEE Cliente = db.OPE_CLIENTPROVEE.Find(id);

            try
            {
                db.OPE_CLIENTPROVEE.Remove(Cliente);
                db.SaveChanges();
                results.Result = true;
                results.Mensaje = "Cliente Eliminado Correctamente";
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
