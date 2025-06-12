using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Domain.Guides.Events;
using MediatR;

namespace EchoesOfUzbekistan.Application.Files;
internal class EntityFileResourceDeletedEventHandler : INotificationHandler<EntityFileResourceDeletedEvent>
{
    private readonly IFileService _fileService;

    public EntityFileResourceDeletedEventHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task Handle(EntityFileResourceDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _fileService.DeleteAsync(notification.ResourceLink, cancellationToken);
    }
}
