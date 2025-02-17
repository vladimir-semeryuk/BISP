namespace EchoesOfUzbekistan.Api.Controllers.AudioGuides;

public record UpdateAudioGuideRequest(
    Guid GuideId,
    string Title,
    string? Description,
    string City,
    decimal MoneyAmount,
    string CurrencyCode,
    Guid LanguageId,
    string GuideStatus,
    string? AudioLink,
    string? ImageLink);
