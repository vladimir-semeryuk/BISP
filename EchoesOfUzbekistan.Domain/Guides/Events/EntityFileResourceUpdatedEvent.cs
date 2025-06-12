using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Guides.Events;
public record EntityFileResourceUpdatedEvent(string ResourceLink) : IDomainEvent;
