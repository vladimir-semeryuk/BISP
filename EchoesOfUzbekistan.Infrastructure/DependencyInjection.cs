using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Users;
using EchoesOfUzbekistan.Infrastructure.Auth;
using EchoesOfUzbekistan.Infrastructure.Authorisation;
using EchoesOfUzbekistan.Infrastructure.Data;
using EchoesOfUzbekistan.Infrastructure.FileUpload;
using EchoesOfUzbekistan.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DATABASE 
        var connectionString = configuration.GetConnectionString("MainDb") 
            ?? throw new ArgumentNullException(nameof(configuration));
        services.AddDbContext<AppDbContext>(options =>
        {
            // using snake case to comply with Postgres naming style conventions
            options.UseNpgsql(connectionString, x => x.UseNetTopologySuite()).UseSnakeCaseNamingConvention();
        });

        services.AddSingleton<ISQLConnectionFactory>(f => new SQLConnectionFactory(connectionString));
        
        // REPOSITORIES 
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IGuideRepository, AudioGuideRepository>();
        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<IUnitOfWork>(p => p.GetRequiredService<AppDbContext>());

        // AUTHENTICATION
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
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContextService, UserContextService>();

        // FILE UPLOADS
        services.Configure<R2Settings>(configuration.GetSection("R2Settings"));
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var r2Settings = sp.GetRequiredService<IOptions<R2Settings>>().Value;
            var accessKey = r2Settings.AccessKey;
            var secretKey = r2Settings.SecretAccessKey;

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var config = new AmazonS3Config
            {
                ServiceURL = r2Settings.ServiceUrl,
                SignatureVersion = "v4"
            };
            AWSConfigsS3.UseSignatureVersion4 = true;

            return new AmazonS3Client(credentials, config);
        });
        services.AddScoped<IFileService, S3FileService>();


        // AUTHORISATION
        services.AddScoped<AuthorisationService>();
        services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

        return services;
    }
}
