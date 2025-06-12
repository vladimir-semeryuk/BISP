using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Domain.Places.Events;
using MediatR;

namespace EchoesOfUzbekistan.Application.Places.DeletePlace;
internal class PlaceDeletedDomainEventHandler : INotificationHandler<PlaceDeletedDomainEvent>
{
    private readonly IFileService _fileService;

    public PlaceDeletedDomainEventHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task Handle(PlaceDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var filesToDelete = new List<string>();

        if (!string.IsNullOrEmpty(notification.ImageLink))
        {
            filesToDelete.Add(notification.ImageLink);
        }

        if (!string.IsNullOrEmpty(notification.AudioLink))
        {
            filesToDelete.Add(notification.AudioLink);
        }

        foreach (var translation in notification.Translations)
        {
            if (!string.IsNullOrEmpty(translation.audioLink?.Value))
            {
                filesToDelete.Add(translation.audioLink.Value);
            }
        }

        // If there are files to delete, delete them in batch
        if (filesToDelete.Any())
        {
            await _fileService.DeleteBatchAsync(filesToDelete, cancellationToken);
        }
    }
}
