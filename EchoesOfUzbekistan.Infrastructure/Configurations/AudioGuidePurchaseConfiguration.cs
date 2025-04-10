using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;
public class GuidePurchaseConfiguration : IEntityTypeConfiguration<AudioGuidePurchase>
{
    public void Configure(EntityTypeBuilder<AudioGuidePurchase> builder)
    {
        // Table name
        builder.ToTable("audio_guide_purchases");

        // Primary Key
        builder.HasKey(gp => new { gp.UserId, gp.GuideId });

        // Relationships
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(gp => gp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<AudioGuide>()
            .WithMany()
            .HasForeignKey(gp => gp.GuideId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: Choose delete behavior

        // Other configurations
        builder.Property(gp => gp.PurchaseDate)
            .IsRequired();
    }
}
