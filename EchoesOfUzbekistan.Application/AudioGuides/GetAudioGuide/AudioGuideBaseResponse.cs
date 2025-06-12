namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;

public abstract class AudioGuideBaseResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public string City { get; init; }
    public decimal PriceAmount { get; init; }
    public string PriceCurrency { get; init; }
    public string Status { get; init; }
    public DateTime DatePublished { get; init; }
    public DateTime? DateEdited { get; init; }
    public Guid AuthorId { get; init; }
    public Guid OriginalLanguageId { get; init; }
    public string? AudioKey { get; init; }
    public string? ImageKey { get; init; }
    public string? AudioLink { get; internal set; } = null;
    public string? ImageLink { get; internal set; } = null;
}