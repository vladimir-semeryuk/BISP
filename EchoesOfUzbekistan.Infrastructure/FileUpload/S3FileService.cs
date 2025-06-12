using Amazon.S3.Model;
using Amazon.S3;
using Amazon.S3.Transfer;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using Microsoft.Extensions.Options;

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

    public async Task UploadAsync(
        byte[] file, 
        string fileName, 
        string filePath, 
        string contentType, 
        CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream(file);

        var putRequest = new PutObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = filePath,
            InputStream = memoryStream,
            ContentType = contentType,
            AutoCloseStream = true,
            DisablePayloadSigning = true
        };

        await _s3Client.PutObjectAsync(putRequest, cancellationToken);
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

    public async Task DeleteBatchAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        if (keys == null || !keys.Any())
            return;

        var deleteObjectsRequest = new DeleteObjectsRequest
        {
            BucketName = _s3Settings.BucketName,
            Objects = keys.Select(k => new KeyVersion { Key = k }).ToList()
        };

        await _s3Client.DeleteObjectsAsync(deleteObjectsRequest, cancellationToken);
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

    public async Task<Stream?> GetStreamAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _s3Settings.BucketName,
                Key = key
            };

            var response = await _s3Client.GetObjectAsync(request, cancellationToken);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }
}