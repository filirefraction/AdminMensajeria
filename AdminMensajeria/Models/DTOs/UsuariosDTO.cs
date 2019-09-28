using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminMensajeria.Models.DTOs
{
    public class UsuariosDTO
    {
        public int Id { get; set; }
        public int TipoUsuario { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public bool Estatus { get; set; }

    }
}