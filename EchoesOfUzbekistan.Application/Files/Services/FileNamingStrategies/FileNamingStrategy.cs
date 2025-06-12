using EchoesOfUzbekistan.Application.Abstractions;

namespace EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;
public class FileNamingStrategyFactory
{
    public static IFileNamingStrategy GetStrategy(EntityTypes entityType)
    {
        switch (entityType)
        {
            case EntityTypes.Guide:
                return new GuideNamingStrategy();
            case EntityTypes.Place:
                return new PlaceNamingStrategy();
            case EntityTypes.UserProfile:
                return new UserNamingStrategy();
            default:
                throw new ArgumentException("Invalid entity type.", nameof(entityType));
        }
    }
}
