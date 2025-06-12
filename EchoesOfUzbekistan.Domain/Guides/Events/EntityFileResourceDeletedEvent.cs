using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Guides.Events;
public record EntityFileResourceDeletedEvent(string ResourceLink) : IDomainEvent;
