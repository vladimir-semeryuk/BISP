namespace EchoesOfUzbekistan.Api.Controllers.AudioGuides;

public record CreateAudioGuideRequest(
    string Title,
    string? Description,
    string City,
    decimal MoneyAmount,
    string CurrencyCode,
    Guid LanguageId,
    Guid AuthorId,
    DateTime DatePublished,
    string? AudioLink,
    string? ImageLink);
