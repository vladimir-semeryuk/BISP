using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.PostAudioGuide;
internal class PostAudioGuideCommandHandler : ICommandHandler<PostAudioGuideCommand, Guid>
{
    public async Task<Result<Guid>> Handle(PostAudioGuideCommand request, CancellationToken cancellationToken)
    {
        return null; // TODO: Add implementation
    }
}
