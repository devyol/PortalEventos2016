﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UI.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EntitiesEvento : DbContext
    {
        public EntitiesEvento()
            : base("name=EntitiesEvento")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CORRELATIVO> CORRELATIVO { get; set; }
        public DbSet<INSCRIPCION> INSCRIPCION { get; set; }
        public DbSet<OPCION_EVENTO> OPCION_EVENTO { get; set; }
        public DbSet<PARENTESCO> PARENTESCO { get; set; }
        public DbSet<PARTICIPANTE> PARTICIPANTE { get; set; }
        public DbSet<PARTICIPANTE_CONTACTO> PARTICIPANTE_CONTACTO { get; set; }
        public DbSet<EVENTO> EVENTO { get; set; }
        public DbSet<INSCRIPCION_OPCION> INSCRIPCION_OPCION { get; set; }
        public DbSet<MOVIMIENTOS> MOVIMIENTOS { get; set; }
    }
}