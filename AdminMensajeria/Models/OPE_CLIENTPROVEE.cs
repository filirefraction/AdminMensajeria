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
    
    public partial class OPE_CLIENTPROVEE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OPE_CLIENTPROVEE()
        {
            this.GEN_USUARIO = new HashSet<GEN_USUARIO>();
            this.OPE_CONTACTO = new HashSet<OPE_CONTACTO>();
            this.OPE_SOLICITUD = new HashSet<OPE_SOLICITUD>();
            this.OPE_PUNTOENTREC = new HashSet<OPE_PUNTOENTREC>();
        }
    
        public int IdClientProvee { get; set; }
        public Nullable<int> TipoClientProvee { get; set; }
        public string RazonSocial { get; set; }
        public string RFC { get; set; }
        public string ObsClientProvee { get; set; }
        public bool EstatusClientProvee { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GEN_USUARIO> GEN_USUARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPE_CONTACTO> OPE_CONTACTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPE_SOLICITUD> OPE_SOLICITUD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPE_PUNTOENTREC> OPE_PUNTOENTREC { get; set; }
    }
}