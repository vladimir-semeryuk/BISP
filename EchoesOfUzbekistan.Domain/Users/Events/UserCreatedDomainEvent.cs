using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Users.Events;
public record UserCreatedDomainEvent(Guid userId) : IDomainEvent;
