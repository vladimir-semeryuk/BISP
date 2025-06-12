using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Guides.Repositories;

namespace EchoesOfUzbekistan.Application.AudioGuides.DeleteAudioGuide;
internal class DeleteAudioGuideCommandHandler : ICommandHandler<DeleteAudioGuideCommand>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public DeleteAudioGuideCommandHandler(IGuideRepository guideRepository, IUserContextService userContextService, IUnitOfWork unitOfWork)
    {
        _guideRepository = guideRepository;
        _userContextService = userContextService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAudioGuideCommand request, CancellationToken cancellationToken)
    {
        var guide = await _guideRepository.GetByIdAsync(request.AudioGuideId, cancellationToken);

        if (guide == null)
        {
            return Result.Failure(AudioGuideErrors.NotFound);
        }

        var hasPermission = AuthorizationGuard.EnsureUserOwnsResource(_userContextService.UserId, guide.AuthorId);
        if (hasPermission.IsFailure && AuthorizationGuard.EnsureUserIsAdminModerator(_userContextService).IsFailure)
        {
            return Result.Failure(hasPermission.Error);
        }

        guide.Delete();
        _guideRepository.Delete(guide);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
