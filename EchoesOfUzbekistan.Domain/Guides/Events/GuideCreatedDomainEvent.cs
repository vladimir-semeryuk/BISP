using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Guides.Events;
public record GuideCreatedDomainEvent(Guid guideId) : IDomainEvent;