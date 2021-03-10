using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


namespace WebApplication1.Data
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AudioModel>().HasData(
                new AudioModel
                {
                    Id = 1,
                    Name = "Carrot Cake"
                },
                new AudioModel
                {
                    Id = 2,
                    Name = "Lemon Tart"
                }
            );

            return modelBuilder;
        }
    }
}
