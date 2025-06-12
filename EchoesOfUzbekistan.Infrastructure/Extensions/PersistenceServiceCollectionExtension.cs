using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.AudioGuides.Interfaces;
using EchoesOfUzbekistan.Application.Comments.Interfaces;
using EchoesOfUzbekistan.Application.Likes.Interfaces;
using EchoesOfUzbekistan.Application.Places.Interfaces;
using EchoesOfUzbekistan.Application.Reports.Interfaces;
using EchoesOfUzbekistan.Application.Users.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Comments;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides.Repositories;
using EchoesOfUzbekistan.Domain.Likes;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Reports;
using EchoesOfUzbekistan.Domain.Users;
using EchoesOfUzbekistan.Infrastructure.Data;
using EchoesOfUzbekistan.Infrastructure.Repositories;
using EchoesOfUzbekistan.Infrastructure.Repositories.AudioGuides;
using EchoesOfUzbekistan.Infrastructure.Repositories.Comments;
using EchoesOfUzbekistan.Infrastructure.Repositories.Likes;
using EchoesOfUzbekistan.Infrastructure.Repositories.Places;
using EchoesOfUzbekistan.Infrastructure.Repositories.Reports;
using EchoesOfUzbekistan.Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EchoesOfUzbekistan.Infrastructure.Extensions;
public static class PersistenceServiceCollectionExtension
{
    public static IServiceCollection AddAppPersistence(this IServiceCollection services, IConfiguration configuration)
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
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IGuideRepository, AudioGuideRepository>();
        services.AddScoped<IGuideReadRepository, DapperAudioGuideReadRepository>();
        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<IPlaceReadRepository, PlaceReadRepository>();
        services.AddScoped<IGuidePurchaseRepository, GuidePurchaseRepository>();
        services.AddScoped<IGuidePurchaseReadRepository, GuidePurchaseReadRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<ILikeReadRepository, LikeReadRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentReadRepository, CommentReadRepository>();
        services.AddScoped<IInappropriateContentReportRepository, InappropriateContentReportRepository>();
        services.AddScoped<IReportReadRepository, ReportReadRepository>();
        services.AddScoped<IUnitOfWork>(p => p.GetRequiredService<AppDbContext>());

        return services;
    }
}
