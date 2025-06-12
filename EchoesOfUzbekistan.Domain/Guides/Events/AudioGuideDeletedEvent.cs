using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Places;

namespace EchoesOfUzbekistan.Domain.Guides.Events;
public record AudioGuideDeletedEvent(
    Guid AudioGuideId, 
    string? AudioLink, 
    string? ImageLink, 
    List<string?> TranslationAudioLinks,
    List<Place> Places) : IDomainEvent;
