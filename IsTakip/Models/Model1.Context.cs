﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IsTakip.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DbIsTakipEntities1 : DbContext
    {
        public DbIsTakipEntities1()
            : base("name=DbIsTakipEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TBL_BIRIMLER> TBL_BIRIMLER { get; set; }
        public virtual DbSet<TBL_DURUM> TBL_DURUM { get; set; }
        public virtual DbSet<TBL_ISLER> TBL_ISLER { get; set; }
        public virtual DbSet<TBL_PERSON> TBL_PERSON { get; set; }
        public virtual DbSet<TBL_YETKILER> TBL_YETKILER { get; set; }
        public virtual DbSet<TBL_MESAJLAR> TBL_MESAJLAR { get; set; }
        public virtual DbSet<TBL_KONUM> TBL_KONUM { get; set; }
    }
}
