using AdminMensajeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AdminMensajeria.Controllers
{
    public class HomeController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        public ActionResult Index()
        {
            GEN_IMAGEN genImagen = this.db.GEN_IMAGEN.FirstOrDefault<GEN_IMAGEN>();
            ViewBag.Imagen = genImagen.Imagen;
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
            Resultados resultados = new Resultados();
            DbSet<OPE_SOLICITUD> opeSolicitud = this.db.OPE_SOLICITUD;
            Expression<Func<OPE_SOLICITUD, bool>> predicate = (Expression<Func<OPE_SOLICITUD, bool>>)(s => s.IdSolicitud == id);
            resultados.Result = opeSolicitud.Where<OPE_SOLICITUD>(predicate).ToList<OPE_SOLICITUD>().Count > 0;
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteFolio(string id)
        {
            Resultados resultados = new Resultados();
            DbSet<OPE_SOLICITUD> opeSolicitud = this.db.OPE_SOLICITUD;
            Expression<Func<OPE_SOLICITUD, bool>> predicate = (Expression<Func<OPE_SOLICITUD, bool>>)(s => s.Folio == id);
            resultados.Result = opeSolicitud.Where<OPE_SOLICITUD>(predicate).ToList<OPE_SOLICITUD>().Count > 0;
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Iniciar(string uxu, string C0m)
        {
            Resultados resultados = new Resultados();
            try
            {
                List<GEN_USUARIO> list = this.db.GEN_USUARIO.Where<GEN_USUARIO>((Expression<Func<GEN_USUARIO, bool>>)(x => x.Usuario == uxu && x.Contrasena == C0m)).ToList<GEN_USUARIO>();
                if (list.Count > 0)
                {
                    if (list[0].EstatusUsuario)
                    {
                        this.Session["IdUsuario"] = (object)list[0].IdUsuario;
                        resultados.Result = true;
                    }
                    else
                    {
                        resultados.Result = false;
                        resultados.Mensaje = "Usuario Inactivo";
                    }
                }
                else
                {
                    resultados.Result = false;
                    resultados.Mensaje = "Usuario o Contraseña Incorrectos";
                }
            }
            catch (Exception ex)
            {
                resultados.Result = false;
                resultados.Mensaje = "Error al iniciar sesión" + ex.Message;
            }
            return this.Json((object)resultados, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cerrar()
        {
            try
            {
                this.Session.Abandon();
                return (ActionResult)this.RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("Index", "Home");
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
            bool flag2 = this.db.SIS_CONFIG.Where<SIS_CONFIG>((Expression<Func<SIS_CONFIG, bool>>)(u => u.IdConfig == 1)).Select<SIS_CONFIG, bool>((Expression<Func<SIS_CONFIG, bool>>)(u => u.AplicaConfig)).FirstOrDefault<bool>();


            if (cambiar == true)
                ViewBag.cambiar = 1;
            else
                ViewBag.cambiar = 0;

            if (flag2)
                ViewBag.ctrlFolio = 1;
            else
                ViewBag.ctrlFolio = 0;

            return View();
        }

        public ActionResult Configuracion()
        {

            return View();
        }
    }
}