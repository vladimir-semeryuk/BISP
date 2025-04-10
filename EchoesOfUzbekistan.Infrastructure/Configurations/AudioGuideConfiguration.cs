using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;
public class AudioGuideConfiguration : IEntityTypeConfiguration<AudioGuide>
{
    public void Configure(EntityTypeBuilder<AudioGuide> builder)
    {
        // Configure primary key
        builder.HasKey(ag => ag.Id);

        // Configure properties
        builder.Property(ag => ag.Title)
            .HasConversion(title => title.guideTitle, guideTitle => new GuideTitle(guideTitle))
            .IsRequired();

        builder.Property(ag => ag.Description)
            .HasMaxLength(5000)
            .IsRequired(false)
            .HasConversion(desc => desc != null ? desc.value : "", value => new GuideInfo(value));

        builder.OwnsOne(ag => ag.Price, priceConverter =>
        {
            priceConverter.Property(money => money.Currency)
                           .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });

        builder.Property(ag => ag.Status)
            .HasConversion(
            status => status.ToString(),
            status => (GuideStatus)Enum.Parse(typeof(GuideStatus), status))
            .IsRequired();

        builder.Property(ag => ag.DatePublished)
            .IsRequired();

        builder.Property(ag => ag.DateEdited)
            .IsRequired(false);

        builder.Property(ag => ag.City)
            .HasConversion(city => city != null ? city.value : null, city => city != null ? new City(city) : null)
            .IsRequired(false);

        builder.Property(ag => ag.AudioLink)
            .HasConversion(audio => audio != null ? audio.value : null, link => link != null ? new ResourceLink(link) : null)
            .IsRequired(false);

        builder.Property(ag => ag.ImageLink)
            .HasConversion(img => img != null ? img.value : null, link => link != null ? new ResourceLink(link) : null)
            .IsRequired(false);

        builder.OwnsMany(ag => ag.Translations, translation =>
        {
            translation.WithOwner().HasForeignKey(t => t.AudioGuideId);
            translation.Property(t => t.AudioGuideId).IsRequired();
            translation.Property(t => t.title).HasConversion(
                v => v.ToString(),
                v => new GuideTitle(v));
            translation.Property(ag => ag.audioLink)
                .HasConversion(audio => audio != null ? audio.value : null, 
                               link => link != null ? new ResourceLink(link) : null);
            translation.HasOne<Language>()
            .WithMany()
            .HasForeignKey(t => t.languageId);
            translation.Property(t => t.description).HasConversion(
                v => v == null ? null : v.ToString(),
                v => v == null ? null : new GuideInfo(v));
        });


        // Relationships 
        builder
            .HasOne<Language>()
            .WithMany()
            .HasForeignKey(ag => ag.OriginalLanguageId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(ag => ag.AuthorId);

        builder
            .HasMany<Place>()
            .WithMany();

        builder.ToTable("audio_guides");
    }
}
