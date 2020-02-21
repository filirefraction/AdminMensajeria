using AdminMensajeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminMensajeria.Controllers
{
    public class HomeController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        public ActionResult Index()
        {
            GEN_IMAGEN img = db.GEN_IMAGEN.FirstOrDefault();
            ViewBag.Imagen = img.Imagen; 
            return View();
        }

        public ActionResult Cambiar(int id)
        {
            ViewBag.IdUsuario = id;
            return PartialView("~/Views/Home/Cambiar.cshtml");
        }

        public JsonResult EditPass(int idusuario, string ps1)
        {
            Resultados resultado = new Resultados();

            GEN_USUARIO Usuario = db.GEN_USUARIO.Find(idusuario);

            Usuario.Contrasena = ps1;
            Usuario.CambiarContrasena = false;

            try
            {
                db.Entry(Usuario).State = EntityState.Modified;
                db.SaveChanges();
                resultado.Result = true;
            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteEnvio(int id)
        {
            Resultados resultado = new Resultados();

            var solicitud = (from s in db.OPE_SOLICITUD where s.IdSolicitud == id select s).ToList();

            if (solicitud.Count > 0)
                resultado.Result = true;
            else
                resultado.Result = false;

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Iniciar(string uxu, string C0m)
        {
            Resultados resultado = new Resultados();

            try
            {
                var inputs = db.GEN_USUARIO.Where(x => x.Usuario == uxu && x.Contrasena == C0m).ToList();

                if (inputs.Count > 0)
                {
                    if (inputs[0].EstatusUsuario == true)
                    {
                        Session["IdUsuario"] = inputs[0].IdUsuario;
                        resultado.Result = true;
                    }
                    else
                    {
                        resultado.Result = false;
                        resultado.Mensaje = "Usuario Inactivo";
                    }
                }
                else
                {
                    resultado.Result = false;
                    resultado.Mensaje = "Usuario o Contraseña Incorrectos";
                }
            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = "Error al iniciar sesión" + ex.Message;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cerrar()
        {
            try
            {
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Estandar()
        {
            int IdUsuario = (int)Session["IdUsuario"];
            ViewBag.idusuario = IdUsuario;
            ViewBag.tipousuario = (from u in db.GEN_USUARIO where u.IdUsuario == IdUsuario select u.TipoUsuario).FirstOrDefault();
            bool cambiar = (from u in db.GEN_USUARIO where u.IdUsuario == IdUsuario select u.CambiarContrasena).FirstOrDefault();

            if (cambiar == true)
                ViewBag.cambiar = 1;
            else
                ViewBag.cambiar = 0;


            return View();
        }

        public ActionResult Configuracion()
        {

            return View();
        }
    }
}