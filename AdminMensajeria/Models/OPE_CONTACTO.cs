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
    
    public partial class OPE_CONTACTO
    {
        public int IdContacto { get; set; }
        public int IdClientProvee { get; set; }
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public Nullable<System.TimeSpan> HoraDe { get; set; }
        public Nullable<System.TimeSpan> HoraHa { get; set; }
        public bool EstatusContacto { get; set; }
    
        public virtual OPE_CLIENTPROVEE OPE_CLIENTPROVEE { get; set; }
    }
}
