﻿using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class KitapCafeContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=UZAY-MEKIGI\\SQLEXPRESS;Database=KitapCafe;User Id=sa;Password=1234;TrustServerCertificate=True;");
        }
        public DbSet<WifiPassword> WifiPasswords { get; set; }
    }
}
