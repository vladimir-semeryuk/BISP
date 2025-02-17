using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Domain.Guides.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.DeleteAudioGuide;
internal class AudioGuideDeletedEventHandler : INotificationHandler<AudioGuideDeletedEvent>
{
    private readonly IFileService _fileService;

    public AudioGuideDeletedEventHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task Handle(AudioGuideDeletedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.TranslationAudioLinks.Any())
        {
            foreach (var link in notification.TranslationAudioLinks)
            {
                if (link != null)
                    await _fileService.DeleteAsync(link);
            }
        }
        if (notification.Places.Any())
        {
            foreach (var place in notification.Places)
            {
                if (place != null)
                    place.MarkAsHidden();
            }
        }
    }
}
