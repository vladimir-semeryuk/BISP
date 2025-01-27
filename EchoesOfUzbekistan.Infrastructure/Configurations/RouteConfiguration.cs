using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Configurations;
public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        // Configure primary key
        builder.HasKey(r => r.Id);

        // Relationships
        builder.HasOne<AudioGuide>()
            .WithMany()
            .HasForeignKey(r => r.AudioGuideId);

        builder.ToTable("routes");
    }
}
