using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EchoesOfUzbekistan.Infrastructure.Extensions;
using Microsoft.Extensions.Hosting;

namespace EchoesOfUzbekistan.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostApplicationBuilder applicationBuilder, IConfiguration configuration)
    {
        services.AddAppPersistence(configuration);

        services.AddAppAuthentication(configuration);

        services.AddFileUploads(configuration);

        services.AddAppAuthorisation(configuration);

        services.AddStripe(configuration);

        services.AddAzureTts(applicationBuilder);

        return services;
    }
}
