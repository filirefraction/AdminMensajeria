//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminMensajeria.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GEN_COMENSUGER
    {
        public int IdComenSuger { get; set; }
        public int IdUsuario { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public System.DateTime FechaCreacionComenSuger { get; set; }
        public string Respuesta { get; set; }
        public Nullable<System.DateTime> FechaAtencionComenSuger { get; set; }
        public bool EstatusComenSuger { get; set; }
    
        public virtual GEN_USUARIO GEN_USUARIO { get; set; }
    }
}
