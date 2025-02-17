using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;

namespace EchoesOfUzbekistan.Application.Files.Services;
public static class MimeMapping
{
    private static readonly Dictionary<string, FileCategories> MimeTypeMappings = new(StringComparer.OrdinalIgnoreCase)
    {
         {"image/png", FileCategories.Image},
         {"image/jpeg", FileCategories.Image},
         {"audio/mpeg", FileCategories.Audio},
         {"audio/wav", FileCategories.Audio}
    };

    public static FileCategories GetFileCategory(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
        {
            return FileCategories.Unknown;
        }

        if (MimeTypeMappings.TryGetValue(contentType, out var category))
        {
            return category;
        }

        if (contentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
        {
            return FileCategories.Image;
        }
        if (contentType.StartsWith("audio", StringComparison.OrdinalIgnoreCase))
        {
            return FileCategories.Audio;
        }

        return FileCategories.Unknown;
    }
}
