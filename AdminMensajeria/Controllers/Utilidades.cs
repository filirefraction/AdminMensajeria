using AdminMensajeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminMensajeria.Controllers
{
    public class Utilidades : Controller
    {
        private MensajeriaEntities db = new MensajeriaEntities();

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

        public JsonResult ListaMunicipios(int id)
        {
            var Municpios = (from e in db.GEN_MUNICIPIO
                           where e.IdEstado == id
                           select new
                           {
                               idmunicipio = e.IdMunicipio,
                               municipio = e.Municipio
                           }).ToList();

            return Json(Municpios, JsonRequestBehavior.AllowGet);
        }

    }
}