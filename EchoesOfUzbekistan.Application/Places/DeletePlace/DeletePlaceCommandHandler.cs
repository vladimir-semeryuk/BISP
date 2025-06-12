using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Places;

namespace EchoesOfUzbekistan.Application.Places.DeletePlace;
internal class DeletePlaceCommandHandler : ICommandHandler<DeletePlaceCommand>
{
    private readonly IUserContextService _userContext;
    private readonly IPlaceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePlaceCommandHandler(IPlaceRepository repository, IUserContextService userContext, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _repository.GetByIdAsync(request.PlaceId, cancellationToken);

        if (place == null)
        {
            return Result.Failure(PlaceErrors.NotFound);
        }

        var hasPermission = AuthorizationGuard.EnsureUserOwnsResource(_userContext.UserId, place.AuthorId);
        if (hasPermission.IsFailure && AuthorizationGuard.EnsureUserIsAdminModerator(_userContext).IsFailure)
        {
            return Result.Failure(hasPermission.Error);
        }

        place.Delete();
        _repository.Delete(place);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
