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
    
    public partial class GEN_MUNICIPIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GEN_MUNICIPIO()
        {
            this.GEN_COLONIA = new HashSet<GEN_COLONIA>();
        }
    
        public int IdMunicipio { get; set; }
        public int IdEstado { get; set; }
        public string Municipio { get; set; }
    
        public virtual GEN_ESTADO GEN_ESTADO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GEN_COLONIA> GEN_COLONIA { get; set; }
    }
}