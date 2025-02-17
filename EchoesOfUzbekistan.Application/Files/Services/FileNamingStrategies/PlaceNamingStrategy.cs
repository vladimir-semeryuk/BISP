using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;
public class PlaceNamingStrategy : IFileNamingStrategy
{
    public string GetFilePath(string userId, string fileId, string contentType)
    {
        var category = MimeMapping.GetFileCategory(contentType);

        switch (category)
        {
            case FileCategories.Image:
                return $"files/{userId}/places/pictures/{fileId}";
            case FileCategories.Audio:
                return $"files/{userId}/places/audio/{fileId}";
            default:
                throw new ArgumentException("Unsupported content type for place.");
        }
    }
}
