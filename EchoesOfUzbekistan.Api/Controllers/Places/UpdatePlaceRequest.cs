namespace EchoesOfUzbekistan.Api.Controllers.Places;

public record UpdatePlaceRequest(
    Guid PlaceId,
    string Title,
    string? Description,
    double Longitude,
    double Latitude,
    string LanguageCode,
    Guid AuthorId,
    string? AudioLink,
    string? ImageLink);