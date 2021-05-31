using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminMensajeria.Models.APP
{
    public class SolicitudUpdate
    {
        public int IdGuia { get; set; }

        public string Receptor { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public string Img { get; set; }

        public string Sign { get; set; }

        public string Problema { get; set; }

        public bool IsComplaint { get; set; }

        public IEnumerable<SolicitudApp> solicitudes { get; set; }
    }
}