using EchoesOfUzbekistan.Domain.Common;

namespace EchoesOfUzbekistan.Domain.Guides;
public record GuideTranslation(
    Guid LanguageId, 
    GuideTitle Title, 
    GuideInfo? Description,
    ResourceLink? AudioLink,
    Guid AudioGuideId);
