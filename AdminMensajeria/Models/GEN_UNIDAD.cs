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
    
    public partial class GEN_UNIDAD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GEN_UNIDAD()
        {
            this.OPE_SOLICITUDPRODUCTO = new HashSet<OPE_SOLICITUDPRODUCTO>();
        }
    
        public int IdUnidad { get; set; }
        public string DescripcionUnidad { get; set; }
        public string Codigo { get; set; }
        public string Simbolo { get; set; }
        public bool EstatusUnidad { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPE_SOLICITUDPRODUCTO> OPE_SOLICITUDPRODUCTO { get; set; }
    }
}
