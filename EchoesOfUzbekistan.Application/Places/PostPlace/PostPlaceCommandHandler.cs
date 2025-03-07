using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.AudioGuides.PostAudioGuide;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Places.PostPlace;
public class PostPlaceCommandHandler : ICommandHandler<PostPlaceCommand, Guid>
{
    private readonly ILanguageRepository languageRepository;

    public PostPlaceCommandHandler(ILanguageRepository languageRepository)
    {
        this.languageRepository = languageRepository;
    }

    public Task<Result<Guid>> Handle(PostPlaceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
