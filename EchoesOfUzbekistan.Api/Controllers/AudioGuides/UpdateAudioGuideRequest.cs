namespace EchoesOfUzbekistan.Api.Controllers.AudioGuides;

public record UpdateAudioGuideRequest(
    Guid GuideId,
    string Title,
    string? Description,
    string City,
    decimal MoneyAmount,
    string CurrencyCode,
    string LanguageCode,
    string? GuideStatus,
    string? AudioLink,
    string? ImageLink,
    List<Guid> PlacesIds);
