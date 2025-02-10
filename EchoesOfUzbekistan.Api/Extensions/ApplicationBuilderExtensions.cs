using EchoesOfUzbekistan.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EchoesOfUzbekistan.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();   
    }
}
