using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;


namespace WebApplication1.Data.Configurations
{
    public class DatabaseFileConfiguration : IEntityTypeConfiguration<DatabaseFile>
    {
        public void Configure(EntityTypeBuilder<DatabaseFile> builder)
        {
            builder.Property(p => p.FileID).HasColumnName("FileIDNumber");
            builder.Property(p => p.ContentType).HasColumnName("FileContentType");
            builder.Property(p => p.FileContent).HasColumnName("FileContent");
            builder.Property(p => p.FileExtension).HasColumnName("FileExtensionType");
            builder.Property(p => p.FileName).HasColumnName("Name");
            builder.Property(p => p.FileSize).HasColumnName("SizeOfFile");
        }
    }
}
