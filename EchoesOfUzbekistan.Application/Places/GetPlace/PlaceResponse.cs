using EchoesOfUzbekistan.Domain.Places;

namespace EchoesOfUzbekistan.Application.Places.GetPlace;
public class PlaceResponse
{
    public Guid PlaceId { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public string Coordinates { get; init; }
    public PlaceStatus Status { get; init; }
    public DateTime DatePublished { get; init; }
    public DateTime? DateEdited { get; init; }
    public Guid AuthorId { get; init; }
    public Guid OriginalLanguageId { get; init; }
    public string LanguageCode { get; set; }
    public string? AudioLink { get; set; } = null;
    public string? ImageLink { get; set; } = null;
    public string? ImageKey { get; init; }
    public string? AudioKey { get; init; }
    public Guid[] AudioGuideIds { get; init; }
    // public ICollection<PlaceTranslation> Translations { get; private set; }
}
