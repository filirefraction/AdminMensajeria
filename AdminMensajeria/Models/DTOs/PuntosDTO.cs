using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminMensajeria.Models.DTOs
{
    public class PuntosDTO
    {
        public int Id { get; set; }

        public int IdSolicitud { get; set; }

        public string FirmaPuntosEntRec { get; set; }

        public string FotoPuntosEntRec { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public bool Problema { get; set; }

        public string ObsProblema { get; set; }

        public bool Estatus { get; set; }
    }

}