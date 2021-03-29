using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Data.Configurations
{
    public class DatabaseFileConfiguration : IEntityTypeConfiguration<DatabaseFile>
    {
        public void Configure(EntityTypeBuilder<DatabaseFile> builder)
        {
            builder.Property(p => p.FileID).HasColumnName("FileID");
        }
    }
}

