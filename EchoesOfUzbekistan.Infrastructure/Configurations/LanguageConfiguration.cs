using EchoesOfUzbekistan.Domain.Guides;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoesOfUzbekistan.Domain.Common;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;
public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        // Configure primary key
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Code)
            .IsRequired()
            .HasMaxLength(2)
            .IsFixedLength();  // ISO codes are typically fixed length (e.g., "en", "es")

        builder.Property(l => l.Name)
            .IsRequired()  
            .HasMaxLength(100);

        builder.HasIndex(l => l.Code)
            .IsUnique();  // Ensure that the Code is unique in the database

        builder.ToTable("languages");
    }
}

