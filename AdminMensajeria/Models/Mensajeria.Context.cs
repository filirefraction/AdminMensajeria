﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MensajeriaEntities : DbContext
    {
        public MensajeriaEntities()
            : base("name=MensajeriaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GEN_CODIGOPOSTAL> GEN_CODIGOPOSTAL { get; set; }
        public virtual DbSet<GEN_COLONIA> GEN_COLONIA { get; set; }
        public virtual DbSet<GEN_COMENSUGER> GEN_COMENSUGER { get; set; }
        public virtual DbSet<GEN_EMAILCONGIF> GEN_EMAILCONGIF { get; set; }
        public virtual DbSet<GEN_ESTADO> GEN_ESTADO { get; set; }
        public virtual DbSet<GEN_MUNICIPIO> GEN_MUNICIPIO { get; set; }
        public virtual DbSet<GEN_PAIS> GEN_PAIS { get; set; }
        public virtual DbSet<GEN_UNIDAD> GEN_UNIDAD { get; set; }
        public virtual DbSet<GEN_USUARIO> GEN_USUARIO { get; set; }
        public virtual DbSet<OPE_CATEGORIA> OPE_CATEGORIA { get; set; }
        public virtual DbSet<OPE_CLIENTPROVEE> OPE_CLIENTPROVEE { get; set; }
        public virtual DbSet<OPE_CONTACTO> OPE_CONTACTO { get; set; }
        public virtual DbSet<OPE_GUIA> OPE_GUIA { get; set; }
        public virtual DbSet<OPE_PUNTOENTREC> OPE_PUNTOENTREC { get; set; }
        public virtual DbSet<OPE_SOLICITUD> OPE_SOLICITUD { get; set; }
        public virtual DbSet<OPE_SOLICITUDPRODUCTO> OPE_SOLICITUDPRODUCTO { get; set; }
        public virtual DbSet<OPE_SOLICITUDPUNTOSENTREC> OPE_SOLICITUDPUNTOSENTREC { get; set; }
        public virtual DbSet<SIS_CONFIG> SIS_CONFIG { get; set; }
        public virtual DbSet<GEN_IMAGEN> GEN_IMAGEN { get; set; }
        public virtual DbSet<VIEW_REPORTE> VIEW_REPORTE { get; set; }
    }
}
