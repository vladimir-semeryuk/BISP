using EchoesOfUzbekistan.Domain.Friendships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.HasKey(f => f.Id);

        // Unique constraint on (FollowerId, FolloweeId) to avoid duplicate follow entries
        builder.HasIndex(f => new { f.FollowerId, f.FolloweeId })
            .IsUnique();

        // Define relationships to User
        builder.HasOne(f => f.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete when a follower is deleted

        // Friendship relationship for Followee
        builder
            .HasOne(f => f.Followee)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FolloweeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("friendships", t =>
        {
            t.HasCheckConstraint("CK_Friendship_NoSelfFollow", "\"follower_id\" <> \"followee_id\"");
        });
    }
}
