using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Places.EditPlace;
public record EditPlaceCommand(
    Guid PlaceId, 
    string Title, 
    string? Description, 
    double Latitude, 
    double Longitude, 
    string LanguageCode, 
    string? AudioLink,
    string? ImageLink,
    List<Guid> AudioGuidesIds) : ICommand;