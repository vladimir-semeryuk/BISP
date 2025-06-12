namespace EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;
public class GuideNamingStrategy : IFileNamingStrategy
{
    public string GetFilePath(string userId, string fileId, string contentType)
    {
        var category = MimeMapping.GetFileCategory(contentType);

        switch (category)
        {
            case FileCategories.Image:
                return $"files/{userId}/guides/pictures/{fileId}";
            case FileCategories.Audio:
                return $"files/{userId}/guides/audio/{fileId}";
            default:
                throw new ArgumentException("Unsupported content type for guide.");
        }
    }
}
