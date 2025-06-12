using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.AudioGuides.EditAudioGuide;
public record EditAudioGuideCommand(
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
    List<Guid> PlacesIds
    ) : ICommand;
