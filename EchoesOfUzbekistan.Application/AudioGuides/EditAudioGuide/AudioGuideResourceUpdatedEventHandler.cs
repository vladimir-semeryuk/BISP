using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Domain.Guides.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.EditAudioGuide;
internal class AudioGuideResourceUpdatedEventHandler : INotificationHandler<AudioGuideResourceUpdatedEvent>
{
    private readonly IFileService _fileService;

    public AudioGuideResourceUpdatedEventHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task Handle(AudioGuideResourceUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _fileService.DeleteAsync(notification.ResourceLink);
    }
}
