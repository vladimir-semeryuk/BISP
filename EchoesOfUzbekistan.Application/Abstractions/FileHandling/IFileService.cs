namespace EchoesOfUzbekistan.Application.Abstractions.FileHandling;
public interface IFileService
{
    Task<string> GetPresignedUrlForGetAsync(string key, CancellationToken cancellationToken = default);
    Task<string> GetPresignedUrlForPutAsync(string fileName, string filePath, string contentType, CancellationToken cancellationToken = default);
    Task DeleteAsync(string key, CancellationToken cancellationToken = default);
    Task UploadAsync(byte[] file, string fileName, string filePath, string contentType, CancellationToken cancellationToken);
    Task DeleteBatchAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);
    Task<Stream?> GetStreamAsync(string key, CancellationToken cancellationToken = default);
}
