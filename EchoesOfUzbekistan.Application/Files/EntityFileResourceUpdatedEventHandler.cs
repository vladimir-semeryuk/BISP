using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Domain.Guides.Events;
using MediatR;

namespace EchoesOfUzbekistan.Application.Files;
internal class EntityFileResourceUpdatedEventHandler : INotificationHandler<EntityFileResourceUpdatedEvent>
{
    private readonly IFileService _fileService;

    public EntityFileResourceUpdatedEventHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task Handle(EntityFileResourceUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _fileService.DeleteAsync(notification.ResourceLink);
    }
}
