using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;
using EchoesOfUzbekistan.Infrastructure.Data;
using EchoesOfUzbekistan.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        return services;
    }
}
