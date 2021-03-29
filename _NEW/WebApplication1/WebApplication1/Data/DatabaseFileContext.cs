using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Configurations;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DatabaseFileContext : DbContext
    {
        public DbSet<DatabaseFile> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data source=DatabaseFile.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DatabaseFileConfiguration()).Seed();
        }
    }
}