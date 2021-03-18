using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Configurations;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DatabaseFileContext : DbContext
    {
        public DbSet<DatabaseFile> databaseFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite(@"Data souce = DatabaseFile.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DatabaseFileConfiguration());
        }
    }
}
