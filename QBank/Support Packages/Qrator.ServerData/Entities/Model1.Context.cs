﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Qrator.ServerData.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QBankEntities : DbContext
    {
        public QBankEntities()
            : base("name=QBankEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Examination> Examinations { get; set; }
        public virtual DbSet<PracticeTest> PracticeTests { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
    }
}
