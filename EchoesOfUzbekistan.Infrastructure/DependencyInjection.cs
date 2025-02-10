using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;
using EchoesOfUzbekistan.Infrastructure.Auth;
using EchoesOfUzbekistan.Infrastructure.Data;
using EchoesOfUzbekistan.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MainDb") 
            ?? throw new ArgumentNullException(nameof(configuration));
        services.AddDbContext<AppDbContext>(options =>
        {
            // using snake case to comply with Postgres naming style conventions
            options.UseNpgsql(connectionString, x => x.UseNetTopologySuite()).UseSnakeCaseNamingConvention();
        });

        services.AddSingleton<ISQLConnectionFactory>(f => new SQLConnectionFactory(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork>(p => p.GetRequiredService<AppDbContext>());


        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
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

        return services;
    }
}
