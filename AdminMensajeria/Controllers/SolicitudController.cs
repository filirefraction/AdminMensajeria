using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminMensajeria.Models;
using ZXing;
using ZXing.Common;
using System.Drawing;
using System.Drawing.Imaging;

namespace AdminMensajeria.Controllers
{
    public class SolicitudController : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

        // GET: Solicitud
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
                ViewBag.idusuario = IdUsuario;
                ViewBag.tipousuario = (from u in db.GEN_USUARIO where u.IdUsuario == IdUsuario select u.TipoUsuario).FirstOrDefault();
            }
            return View();
        }

        // GET: Solicitud/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            if (oPE_SOLICITUD == null)
            {
                return HttpNotFound();
            }
            return View(oPE_SOLICITUD);
        }

        // GET: Solicitud/Create
        public ActionResult Create()
        {
            int IdUsuario = 0;
            if (Session["IdUsuario"] == null)
                return RedirectToAction("Index", "Home");
            else
                IdUsuario = (int)Session["IdUsuario"];


            int idusuario = IdUsuario; //aqui va la variable de sesión
            ViewBag.idpuntorec = (from u in db.GEN_USUARIO where u.IdUsuario == idusuario select u.IdPuntoEntRec).FirstOrDefault();
            ViewBag.IdUnidad = new SelectList(db.GEN_UNIDAD, "IdUnidad", "DescripcionUnidad");
            ViewBag.ListCategoria = new SelectList(db.OPE_CATEGORIA, "IdCategoria", "Categoria");
            return View();
        }

        public JsonResult CreateSolicitud(OPE_SOLICITUD Solicitud)
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


            Solicitud.IdUsuario = IdUsuario; // Aqui va Variable de sesión
            Solicitud.FechaCreacion = DateTime.Now;
            Solicitud.Emergencia = false;


            try
            {
                db.OPE_SOLICITUD.Add(Solicitud);
                db.SaveChanges();
                resultado.Result = true;
                resultado.Valor = (from a in db.OPE_SOLICITUD orderby a.IdSolicitud descending select a.IdSolicitud).First();

                //Crea Imagen QR
                var writer = new BarcodeWriter() // Si un barcodeWriter para generar un codigo QR (O.O)
                {
                    Format = BarcodeFormat.QR_CODE, //setearle el tipo de codigo que generara.
                    Options = new EncodingOptions()
                    {
                        Height = 100,
                        Width = 100,
                        Margin = 1, // el margen que tendra el codigo con el restro de la imagen
                    },
                };

                // Generar el codigo, este metodo retorna una bitmap
                Bitmap bitmap = writer.Write(resultado.Valor.ToString());

                // guardar el bitmap con el formato deseado y la locacion deseada
                bitmap.Save(String.Format(Server.MapPath("~/Image/qr/") + "{0}.png", resultado.Valor.ToString()), ImageFormat.Png);

            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = ex.Message;
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateProducto(OPE_SOLICITUDPRODUCTO Producto)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.OPE_SOLICITUDPRODUCTO.Add(Producto);
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

        public JsonResult CreateSolicitudPunto(OPE_SOLICITUDPUNTOSENTREC Punto)
        {
            Resultados resultado = new Resultados();
            try
            {
                db.OPE_SOLICITUDPUNTOSENTREC.Add(Punto);
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

        public ActionResult Puntos(int id)
        {
            ViewBag.idclientprovee = id;
            return PartialView("~/Views/Solicitud/Puntos.cshtml");
        }

        public ActionResult Rastreo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            if (oPE_SOLICITUD == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Views/Solicitud/Rastreo.cshtml", oPE_SOLICITUD);
        }

        public ActionResult Recepcion()
        {
            return PartialView("~/Views/Solicitud/Recepcion.cshtml");
        }
        public JsonResult RecibirPaquete(int id)
        {
            Resultados resultado = new Resultados();

            int IdUsuario;
            string NombreUsuario;
            if (Session["IdUsuario"] == null)
            {
                resultado.Redirecciona = true;
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            else
            {
                IdUsuario = (int)Session["IdUsuario"];
                NombreUsuario = (from n in db.GEN_USUARIO where n.IdUsuario == IdUsuario select n.NombreUsuario).FirstOrDefault();
            }


            OPE_SOLICITUDPRODUCTO Producto = db.OPE_SOLICITUDPRODUCTO.Where(x => x.IdSolicitud == id).FirstOrDefault();
            OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Where(x => x.IdSolicitud == id).FirstOrDefault();


            if (Producto != null)
            {
                if (Producto.Recibido != true)
                {
                    Producto.Recibido = true;
                    Producto.FechaRecepcion = DateTime.Now;
                    Producto.Receptor = NombreUsuario;
                    Solicitud.EstatusSolicitud = 2;
                    try
                    {
                        db.Entry(Producto).State = EntityState.Modified;
                        db.Entry(Solicitud).State = EntityState.Modified;
                        db.SaveChanges();
                        resultado.Result = true;
                        resultado.Mensaje = "El producto de la solicitud " + id.ToString() + " se recibio correctamente " + Producto.Descripcion;

                    }
                    catch (Exception ex)
                    {
                        resultado.Result = false;
                        resultado.Mensaje = "Error al recibir producto: " + ex.Message;
                    }
                }
                else
                {
                    resultado.Result = false;
                    resultado.Mensaje = "Este Producto ya se Recibió";
                }
            }
            else
            {
                resultado.Result = false;
                resultado.Mensaje = "No Existe Numero de Envío";
            }


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportaEntrega(int id)
        {
            ViewBag.idsolicitud = id;
            return PartialView("~/Views/Solicitud/ReportaEntrega.cshtml");
        }
        public JsonResult ReportarEntrega(int id, DateTime FechaEntrega)
        {
            Resultados resultado = new Resultados();
            OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Find(id);
            OPE_SOLICITUDPUNTOSENTREC Entrega = db.OPE_SOLICITUDPUNTOSENTREC.Where(x => x.IdSolicitud == id && x.TipoPunto == 2).FirstOrDefault();

            try
            {
                Solicitud.EstatusSolicitud = 4;
                Entrega.FechaReal = FechaEntrega;
                db.Entry(Solicitud).State = EntityState.Modified;
                db.Entry(Entrega).State = EntityState.Modified;
                db.SaveChanges();
                resultado.Result = true;

            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = "Error al Reportar Entrega Envío: " + ex.Message;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CancelaEnvio(int id)
        {
            Resultados resultado = new Resultados();
            OPE_SOLICITUD Solicitud = db.OPE_SOLICITUD.Find(id);
            Solicitud.EstatusSolicitud = 5;
            try
            {
                db.Entry(Solicitud).State = EntityState.Modified;
                db.SaveChanges();
                resultado.Result = true;

            }
            catch (Exception ex)
            {
                resultado.Result = false;
                resultado.Mensaje = "Error al Cancelar Envío: " + ex.Message;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        // GET: Solicitud/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            if (oPE_SOLICITUD == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUsuario = new SelectList(db.GEN_USUARIO, "IdUsuario", "NombreUsuario", oPE_SOLICITUD.IdUsuario);
            return View(oPE_SOLICITUD);
        }

        // POST: Solicitud/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSolicitud,IdUsuario,IdClientProvee,Folio,FechaCreacion,RequiereAcuse,Acuse,Emergencia,EstatusSolicitud")] OPE_SOLICITUD oPE_SOLICITUD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oPE_SOLICITUD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUsuario = new SelectList(db.GEN_USUARIO, "IdUsuario", "NombreUsuario", oPE_SOLICITUD.IdUsuario);
            return View(oPE_SOLICITUD);
        }

        // GET: Solicitud/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            if (oPE_SOLICITUD == null)
            {
                return HttpNotFound();
            }
            return View(oPE_SOLICITUD);
        }

        // POST: Solicitud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OPE_SOLICITUD oPE_SOLICITUD = db.OPE_SOLICITUD.Find(id);
            db.OPE_SOLICITUD.Remove(oPE_SOLICITUD);
            db.SaveChanges();
            return RedirectToAction("Index");
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

        public JsonResult InformacionPunto(int id)
        {
            var Punto = (from p in db.OPE_PUNTOENTREC
                         where p.IdPuntoEntRec == id
                         select new
                         {
                             idpunto = p.IdPuntoEntRec,
                             descripcion = p.DescripcionPunto,
                             contacto = p.ContactoPunto,
                             calle = p.CallePunto,
                             noext = p.NoExtPunto,
                             noint = p.NoIntPunto,
                             referencia = p.Referencia,
                             cp = p.GEN_COLONIA.GEN_CODIGOPOSTAL.CodigoPostal,
                             idcolonia = p.IdColonia,
                             colonia = p.GEN_COLONIA.Colonia,
                             municipio = p.GEN_COLONIA.GEN_MUNICIPIO.Municipio,
                             estado = p.GEN_COLONIA.GEN_MUNICIPIO.GEN_ESTADO.CiudadEstado,
                             pais = p.GEN_COLONIA.GEN_MUNICIPIO.GEN_ESTADO.GEN_PAIS.Pais
                         }).FirstOrDefault();

            return Json(Punto, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaColonias(string id)
        {
            var Colonias = (from c in db.GEN_COLONIA
                            join p in db.GEN_CODIGOPOSTAL
                            on c.IdCodigoP equals p.IdCodigoP
                            where p.CodigoPostal == id
                            select new
                            {
                                idcolonia = c.IdColonia,
                                colonia = c.Colonia
                            }).ToList();

            return Json(Colonias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ComplementoDomicilio(int id)
        {
            var Complemento = (from c in db.GEN_COLONIA
                               where c.IdColonia == id
                               select new
                               {
                                   municipio = c.GEN_MUNICIPIO.Municipio,
                                   estado = c.GEN_MUNICIPIO.GEN_ESTADO.CiudadEstado,
                                   pais = c.GEN_MUNICIPIO.GEN_ESTADO.GEN_PAIS.Pais
                               }).FirstOrDefault();

            return Json(Complemento, JsonRequestBehavior.AllowGet);
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
