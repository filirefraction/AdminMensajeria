using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminMensajeria.Models.WebAPI
{
    public class EnvioModel
    {
        public string FechaProceso { get; set; }

        public string Referencia { get; set; }

        public int NumEtiquetas { get; set; }

        public string RazonSocial { get; set; }

        public string DireccionCliente { get; set; }

        public string CiudadCliente { get; set; }

        public int TipoEnvio { get; set; }

        public string CodigoCliente { get; set; }
    }
}