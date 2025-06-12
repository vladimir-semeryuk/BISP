using EchoesOfUzbekistan.Infrastructure.Authorisation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EchoesOfUzbekistan.Infrastructure.Extensions;
public static class AuthorisationServiceCollectionExtension
{
    public static IServiceCollection AddAppAuthorisation(this IServiceCollection services, IConfiguration configuration)
    {
        // AUTHORISATION
        services.AddScoped<AuthorisationService>();
        services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

        return services;
    }
}
