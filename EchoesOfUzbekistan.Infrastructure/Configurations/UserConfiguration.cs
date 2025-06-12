using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configure primary key
        builder.HasKey(u => u.Id);

        // Configure properties
        builder.Property(u => u.FirstName)
            .HasConversion(name => name.Value, name => new FirstName(name))
            .IsRequired();

        builder.Property(u => u.Surname)
            .HasConversion(surname => surname.Value, surname => new Surname(surname))
            .IsRequired();

        builder.Property(u => u.Email)
            .HasConversion(email => email.Value, email => new Email(email))
            .IsRequired();

        builder.Property(u => u.RegistrationDateUtc)
            .IsRequired();
        
        builder.Property(u => u.City)
            .HasConversion(city => city != null ? city.Value : null, city => city != null ? new City(city) : null)
            .IsRequired(false);

        builder.Property(u => u.AboutMe)
            .HasConversion(desc => desc != null ? desc.Value : null, desc => desc != null ? new AboutMe(desc) : null);

        builder.Property(u => u.ImageLink)
            .HasConversion(img => img != null ? img.Value : null, link => link != null ? new ResourceLink(link) : null)
            .IsRequired(false);

        builder.OwnsOne(u => u.Country);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasIndex(u => u.IdentityId).IsUnique();

        builder.ToTable("users");
    }
}