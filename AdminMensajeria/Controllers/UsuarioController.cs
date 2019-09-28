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
    public class UsuarioController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Usuario
        public ActionResult Index()
        {
            var gEN_USUARIO = db.GEN_USUARIO.Include(g => g.OPE_PUNTOENTREC);
            return View(gEN_USUARIO.ToList());
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.ListCategoria = new SelectList(db.OPE_CATEGORIA, "IdCategoria", "Categoria");
            return PartialView("~/Views/Usuario/Create.cshtml");
        }

        public JsonResult CreateUsuario(GEN_USUARIO gEN_USUARIO)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.GEN_USUARIO.Add(gEN_USUARIO);
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

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            GEN_USUARIO gEN_USUARIO = db.GEN_USUARIO.Find(id);

            int IdPuntoEntRec = gEN_USUARIO.IdPuntoEntRec;
            int IdCliente = (from c in db.OPE_PUNTOENTREC where c.IdPuntoEntRec == IdPuntoEntRec select c.IdClientProvee).FirstOrDefault();
            int IdCategoria = (from i in db.OPE_CLIENTPROVEE where i.IdClientProvee == IdCliente select i.IdCategoria).FirstOrDefault();
          

            ViewBag.ListPunto = new SelectList(db.OPE_PUNTOENTREC.Where(x => x.IdClientProvee == IdCliente), "IdPuntoEntRec", "DescripcionPunto", IdPuntoEntRec);
            ViewBag.ListCliente = new SelectList(db.OPE_CLIENTPROVEE.Where(x => x.IdCategoria == IdCategoria), "IdClientProvee", "RazonSocial", IdCliente);
            ViewBag.ListCategoria = new SelectList(db.OPE_CATEGORIA, "IdCategoria", "Categoria", IdCategoria);
            return PartialView("~/Views/Usuario/Edit.cshtml", gEN_USUARIO);
        }

        public JsonResult EditUsuario(GEN_USUARIO gEN_USUARIO)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(gEN_USUARIO).State = EntityState.Modified;
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

        public JsonResult DeleteUsuario(int id)
        {
            Resultados results = new Resultados();
            GEN_USUARIO Usuario = db.GEN_USUARIO.Find(id);

            try
            {
                db.GEN_USUARIO.Remove(Usuario);
                db.SaveChanges();
                results.Result = true;
                results.Mensaje = "Usuario Eliminado Correctamente";
            }
            catch (Exception ex)
            {
                results.Result = false;
                results.Mensaje = ex.Message;
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaClientes(int id)
        {
            var Clientes = (from e in db.OPE_CLIENTPROVEE
                            where e.IdCategoria == id
                            select new
                            {
                                idcliente = e.IdClientProvee,
                                cliente = e.RazonSocial
                            }).ToList();

            return Json(Clientes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaPuntos(int id)
        {
            var Puntos = (from e in db.OPE_PUNTOENTREC
                            where e.IdClientProvee == id
                            select new
                            {
                                idpunto = e.IdPuntoEntRec,
                                punto = e.DescripcionPunto
                            }).ToList();

            return Json(Puntos, JsonRequestBehavior.AllowGet);
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
