namespace EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;
public class UserNamingStrategy : IFileNamingStrategy
{
    public string GetFilePath(string userId, string fileId, string contentType)
    {
        var category = MimeMapping.GetFileCategory(contentType);

        switch (category)
        {
            case FileCategories.Image:
                return $"files/{userId}/profile/pictures/{fileId}";
            default:
                throw new ArgumentException("Unsupported content type for user profile.");
        }
    }
}
