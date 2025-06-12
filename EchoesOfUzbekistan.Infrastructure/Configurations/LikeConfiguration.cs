using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EchoesOfUzbekistan.Domain.Likes;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(l => l.Id);

        builder.HasIndex(l => new { l.UserId, l.EntityId, l.EntityType })
            .IsUnique();

        // Relationships
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(l => l.EntityType).IsRequired();

        builder.ToTable("likes");
    }
}
