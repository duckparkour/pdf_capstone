using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Data.Configurations
{
    public class AudioConfiguration : IEntityTypeConfiguration<AudioModel>
    {
        public void Configure(EntityTypeBuilder<AudioModel> builder)
        {
            builder.Property(p => p.Name).HasColumnName("Name");
        }
    }
}
