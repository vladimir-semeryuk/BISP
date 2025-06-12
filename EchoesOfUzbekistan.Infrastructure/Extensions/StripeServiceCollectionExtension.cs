using EchoesOfUzbekistan.Application.Abstractions.Payments;
using EchoesOfUzbekistan.Infrastructure.Payments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace EchoesOfUzbekistan.Infrastructure.Extensions;
public static class StripeServiceCollectionExtension
{
    public static IServiceCollection AddStripe(this IServiceCollection services, IConfiguration configuration)
    {
        // PAYMENT
        var stripeSecretKey = configuration["Stripe:SecretKey"];
        services.AddSingleton(x => new StripeClient(stripeSecretKey));
        StripeConfiguration.ApiKey = stripeSecretKey;
        services.AddScoped<IPaymentProcessor, StripePaymentProcessor>();

        return services;
    }
}
