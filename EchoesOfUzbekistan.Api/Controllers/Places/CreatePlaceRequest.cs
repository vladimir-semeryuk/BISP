namespace EchoesOfUzbekistan.Api.Controllers.Places;

public record CreatePlaceRequest(
    string Title,
    string? Description,
    double Longitude,
    double Latitude,
    string LanguageCode,
    Guid AuthorId,
    string? AudioLink,
    string? ImageLink,
    List<Guid>? AudioGuidesIds);