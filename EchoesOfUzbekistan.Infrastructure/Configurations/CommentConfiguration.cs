using EchoesOfUzbekistan.Domain.Likes;
using EchoesOfUzbekistan.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoesOfUzbekistan.Domain.Comments;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;
public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        // Relationships
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.ToTable("comments");
    }
}
