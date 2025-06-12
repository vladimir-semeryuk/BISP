namespace EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;
public interface IFileNamingStrategy
{
    string GetFilePath(string userId, string fileId, string contentType);
}