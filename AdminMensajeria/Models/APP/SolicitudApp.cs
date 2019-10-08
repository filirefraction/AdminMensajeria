using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminMensajeria.Models.APP
{
    public class SolicitudApp
    {
        public int IdSolicitudPuntosEntRec { get; set; }
        public int IdSolicitud { get; set; }
        public Nullable<int> IdPuntoEntRec { get; set; }
        public Nullable<int> IdGuia { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public bool Problema { get; set; }
        public string ObsProblema { get; set; }
        public bool EstatusPuntosEntRec { get; set; }
    }
}