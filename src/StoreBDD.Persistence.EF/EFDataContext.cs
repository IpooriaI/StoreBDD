﻿using StoreBDD.Entities;
using StoreBDD.Persistence.EF.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Persistence.EF
{
    public class EFDataContext : DbContext
    {

        public EFDataContext(string connectionString) :
            this(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options)
        { }
       
        public EFDataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(CategoryEntityMap).Assembly);
        }
       
        public DbSet<Category> Categories { get; set; }
        
    }
}