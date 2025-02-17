using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;
public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        // Configure primary key
        builder.HasKey(p => p.Id);

        // Configure properties
        builder.Property(p => p.Title)
            .HasConversion(title => title.placeTitle, placeTitle => new PlaceTitle(placeTitle))
            .IsRequired();
        
        builder.Property(p => p.Description)
            .HasMaxLength(5000)
            .IsRequired(false)
            .HasConversion(desc => desc != null ? desc.value : null, value => new PlaceDescription(value));

        builder.Property(p => p.Coordinates)
            .HasColumnType("geography(Point, 4326)"); // Specify the spatial type for PostgreSQL (PostGIS)

        builder.Property(p => p.DatePublished)
            .IsRequired();

        builder.Property(p => p.DateEdited)
            .IsRequired(false);

        builder.Property(p => p.AudioLink)
            .HasConversion(audio => audio != null ? audio.value : null, link => link != null ? new ResourceLink(link) : null)
            .IsRequired(false);

        builder.Property(p => p.ImageLink)
            .HasConversion(img => img != null ? img.value : null, link => link != null ? new ResourceLink(link) : null)
            .IsRequired(false);

        builder.Property(p => p.Status)
            .HasConversion(
            status => status.ToString(),
            status => (PlaceStatus)Enum.Parse(typeof(PlaceStatus), status));

        builder.OwnsMany(p => p.Translations, translation =>
        {
            translation.WithOwner().HasForeignKey(t => t.placeId);
            translation.Property(t => t.placeId).IsRequired();
            translation.Property(t => t.title).HasConversion(
                v => v.ToString(),
                v => new PlaceTitle(v));
            translation.Property(t => t.audioLink)
                .HasConversion(audio => audio != null ? audio.value : null,
                               link => link != null ? new ResourceLink(link) : null);
            translation.HasOne<Language>()
            .WithMany()
            .HasForeignKey(t => t.languageId);
            translation.Property(t => t.description).HasConversion(
                v => v == null ? null : v.ToString(),
                v => v == null ? null : new PlaceDescription(v));
        });

        // Relationships
        builder
            .HasOne<Language>()
            .WithMany()
            .HasForeignKey(p => p.OriginalLanguageId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.AuthorId);

        //builder.Ignore(p => p.Guides);
    }
}
