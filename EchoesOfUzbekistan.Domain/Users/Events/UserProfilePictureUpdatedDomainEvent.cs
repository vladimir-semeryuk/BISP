using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Users.Events;

public record UserProfilePictureUpdatedDomainEvent(Guid Id, string OldImageLink, string NewImageLink) : IDomainEvent;
