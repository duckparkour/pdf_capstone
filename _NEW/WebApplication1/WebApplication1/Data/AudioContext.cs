using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Configurations;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AudioContext : DbContext
    {
        public DbSet<AudioModel> Audios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data source=Audio.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AudioConfiguration()).Seed();
        }
    }
}
