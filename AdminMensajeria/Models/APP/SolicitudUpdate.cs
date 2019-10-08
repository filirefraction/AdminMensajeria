using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminMensajeria.Models.APP
{
    public class SolicitudUpdate
    {
        public int IdGuia { get; set; }
        public String Receptor { get; set; }
        public String Latitud { get; set; }
        public String Longitud { get; set; }
        public String Img { get; set; }
        public String Sign { get; set; }
        public String Problema { get; set; }
        public bool IsComplaint { get; set; }

        public IEnumerable<SolicitudApp> solicitudes { get; set; }
    }
}