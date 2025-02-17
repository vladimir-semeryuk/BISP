using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.DeleteAudioGuide;
internal class DeleteAudioGuideCommandHandler : ICommandHandler<DeleteAudioGuideCommand>
{
    public async Task<Result> Handle(DeleteAudioGuideCommand request, CancellationToken cancellationToken)
    {
        // TODO: Finish the implementation
        return Result.Success();
    }
}
