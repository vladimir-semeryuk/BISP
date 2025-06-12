using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Domain.Users.Events;
using MediatR;

namespace EchoesOfUzbekistan.Application.Users.UpdateUser;
public class UserProfilePictureUpdatedHandler : INotificationHandler<UserProfilePictureUpdatedDomainEvent>
{
    private readonly IFileService _fileService;

    public UserProfilePictureUpdatedHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task Handle(UserProfilePictureUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(domainEvent.OldImageLink))
        {
            await _fileService.DeleteAsync(domainEvent.OldImageLink, cancellationToken);
        }
    }
}