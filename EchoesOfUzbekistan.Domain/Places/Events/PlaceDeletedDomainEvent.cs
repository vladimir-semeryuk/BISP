using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Places.Events;
public record PlaceDeletedDomainEvent(string? ImageLink, string? AudioLink, List<PlaceTranslation> Translations) : IDomainEvent;