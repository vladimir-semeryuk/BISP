using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EchoesOfUzbekistan.Infrastructure.Extensions;
public static class AuthServiceCollectionExtension
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        // AUTHENTICATION
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer
            (
                options =>
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = ctx =>
                        {
                            ctx.Request.Cookies.TryGetValue("access_token", out var accessToken);
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                ctx.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                }
            );
        services.Configure<AuthJwtBearerOptions>(configuration.GetSection("Authentication"));
        services.ConfigureOptions<JwtBearerOptionsConfiguration>();
        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services.AddTransient<AdminAuthHttpDelegatingHandler>();
        services.AddHttpClient<IAuthService, AuthService>((serviceProvider, httpClient) =>
            {
                var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<AdminAuthHttpDelegatingHandler>();

        services.AddHttpClient<ITokenService, TokenService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<ICookieService, CookieService>();

        return services;
    }
}
