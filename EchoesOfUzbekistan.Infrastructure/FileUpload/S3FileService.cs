using Amazon.S3.Model;
using Amazon.S3;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;

namespace EchoesOfUzbekistan.Infrastructure.FileUpload;
public class S3FileService : IFileService
{
    private readonly IAmazonS3 _s3Client;
    private readonly R2Settings _s3Settings;

    public S3FileService(IAmazonS3 s3Client, IOptions<R2Settings> s3Settings)
    {
        _s3Client = s3Client;
        _s3Settings = s3Settings.Value;
    }

    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = key
        };
        
        await _s3Client.DeleteObjectAsync(request);
    }

    public Task<string> GetPresignedUrlForGetAsync(string key, CancellationToken cancellationToken = default)
    {
        // Create the request for a GET presigned URL.
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = key,
            Verb = HttpVerb.GET,
            Protocol = Protocol.HTTPS,
            Expires = DateTime.UtcNow.AddMinutes(15)
        };

        string preSignedUrl = _s3Client.GetPreSignedURL(request);
        return Task.FromResult(preSignedUrl);
    }


    Task<string> IFileService.GetPresignedUrlForPutAsync(string fileName, string filePath, string? contentType, CancellationToken cancellationToken)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = filePath,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(15),
            Metadata =
            {
                ["file-name"] = fileName
            }
        };
        request.ResponseHeaderOverrides.ContentType = contentType;

        // Generate the presigned URL.
        string preSignedUrl = _s3Client.GetPreSignedURL(request);
        return Task.FromResult(preSignedUrl);
    }
}