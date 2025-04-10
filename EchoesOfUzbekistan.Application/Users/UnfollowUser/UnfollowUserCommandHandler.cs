using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Users.UnfollowUser;
public class UnfollowUserCommandHandler : ICommandHandler<UnfollowUserCommand>
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public UnfollowUserCommandHandler(IUserRepository repository, IUserContextService userContext, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _repository.GetByIdAsync(_userContext.UserId, cancellationToken);

        var targetUser = await _repository.GetByIdAsync(request.TargetUserId, cancellationToken);

        if (currentUser == null || targetUser == null)
            return Result.Failure(UserErrors.NotFound);

        currentUser.Unfollow(targetUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
