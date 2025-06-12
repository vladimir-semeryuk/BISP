using Amazon.Runtime;
using Amazon.S3;
using Amazon;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Files.Interfaces;
using EchoesOfUzbekistan.Infrastructure.FileUpload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EchoesOfUzbekistan.Infrastructure.Extensions;
public static class FileUploadServiceCollectionExtension
{
    public static IServiceCollection AddFileUploads(this IServiceCollection services, IConfiguration configuration)
    {
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
                SignatureVersion = "v4",
                // ForcePathStyle = true,
                RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED,
                ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED
            };
            AWSConfigsS3.UseSignatureVersion4 = true;

            return new AmazonS3Client(credentials, config);
        });
        services.AddScoped<IFileService, S3FileService>();
        services.AddScoped<IFileAuthorizationService, FileAuthorizationService>();

        return services;
    }
}
