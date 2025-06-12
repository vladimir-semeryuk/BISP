using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Places.PostPlace;
public record PostPlaceCommand(
    string Title,
    string? Description,
    double Longitude,
    double Latitude,
    string LanguageCode,
    Guid AuthorId,
    string? AudioLink,
    string? ImageLink,
    List<Guid>? AudioGuidesIds) : ICommand<Guid>;
