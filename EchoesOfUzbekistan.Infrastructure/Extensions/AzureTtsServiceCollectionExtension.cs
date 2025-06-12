using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EchoesOfUzbekistan.Application.TextToSpeech.Interfaces;
using EchoesOfUzbekistan.Infrastructure.TextToSpeech;
using Microsoft.Extensions.Hosting;

namespace EchoesOfUzbekistan.Infrastructure.Extensions;

public static class AzureTtsServiceCollectionExtension
{
    public static IServiceCollection AddAzureTts(this IServiceCollection services, IHostApplicationBuilder applicationBuilder)
    {
        // TEXT-TO-SPEECH
        // applicationBuilder.Configuration.AddJsonFile(
        //     Path.Combine(AppContext.BaseDirectory, "Config/voice-mappings.json"),
        //     optional: false,
        //     reloadOnChange: false);

        services.AddScoped<ITextToSpeechService, AzureTextToSpeechService>();

        return services;
    }
}
