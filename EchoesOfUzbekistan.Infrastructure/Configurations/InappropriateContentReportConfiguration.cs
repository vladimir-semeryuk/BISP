using EchoesOfUzbekistan.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EchoesOfUzbekistan.Domain.Reports;
using EchoesOfUzbekistan.Domain.Guides;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;
public class InappropriateContentReportConfiguration : IEntityTypeConfiguration<InappropriateContentReport>
{
    public void Configure(EntityTypeBuilder<InappropriateContentReport> builder)
    {
        // Primary key configuration
        builder.HasKey(r => r.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne<AudioGuide>()
            .WithMany() 
            .HasForeignKey(r => r.AudioGuideId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.Property(r => r.Reason)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.ToTable("inappropriate_content_reports");
    }
}
