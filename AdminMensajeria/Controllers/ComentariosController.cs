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
    public class ComentariosController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Comentarios
        public ActionResult Index()
        {
            int IdUsuario = 0;
            if (Session["IdUsuario"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                IdUsuario = (int)Session["IdUsuario"];
                var TipoUsuario = (from u in db.GEN_USUARIO where u.IdUsuario == IdUsuario select u.TipoUsuario).FirstOrDefault();

                if (TipoUsuario == 1)
                {
                  var gEN_COMENSUGER = (from c in db.GEN_COMENSUGER select c);
                  return View(gEN_COMENSUGER.ToList());
                }
                else
                {
                    var gEN_COMENSUGER = (from c in db.GEN_COMENSUGER where c.IdUsuario == IdUsuario select c);
                    return View(gEN_COMENSUGER.ToList());
                }
            }
        }

        // GET: Comentarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GEN_COMENSUGER gEN_COMENSUGER = db.GEN_COMENSUGER.Find(id);
            if (gEN_COMENSUGER == null)
            {
                return HttpNotFound();
            }
            return View(gEN_COMENSUGER);
        }

        // GET: Comentarios/Create
        public ActionResult Create()
        {
            return PartialView("~/Views/Comentarios/Create.cshtml");
        }

        public JsonResult CreateComentario(GEN_COMENSUGER Comentario)
        {
            Resultados resultado = new Resultados();

            int IdUsuario = 0;
            if (Session["IdUsuario"] == null)
            {
                resultado.Redirecciona = true;
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            else
                IdUsuario = (int)Session["IdUsuario"];


            Comentario.IdUsuario = IdUsuario; //Aqui va variable de sesión
            //Comentario.FechaCreacionComenSuger = DateTime.Now; //Local
            Comentario.FechaCreacionComenSuger = DateTime.Now.AddHours(1); //Producción

            try
            {
                db.GEN_COMENSUGER.Add(Comentario);
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

        // GET: Comentarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GEN_COMENSUGER gEN_COMENSUGER = db.GEN_COMENSUGER.Find(id);
            if (gEN_COMENSUGER == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Comentarios/Edit.cshtml", gEN_COMENSUGER);
        }

        public JsonResult EditComentario(GEN_COMENSUGER Comentario)
        {
            var estatus = (from c in db.GEN_COMENSUGER where c.IdComenSuger == Comentario.IdComenSuger select c.EstatusComenSuger).FirstOrDefault();
            Comentario.EstatusComenSuger = estatus;
            Resultados resultado = new Resultados();
            try
            {
                db.Entry(Comentario).State = EntityState.Modified;
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

        public JsonResult DeleteComentario(int id)
        {
            Resultados results = new Resultados();
            GEN_COMENSUGER Comentario = db.GEN_COMENSUGER.Find(id);

            try
            {
                db.GEN_COMENSUGER.Remove(Comentario);
                db.SaveChanges();
                results.Result = true;
                results.Mensaje = "Comentario Eliminado Correctamente";
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
