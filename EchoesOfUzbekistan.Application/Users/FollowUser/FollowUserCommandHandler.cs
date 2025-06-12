using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Users;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Users.FollowUser;
public class FollowUserCommandHandler : ICommandHandler<FollowUserCommand>
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public FollowUserCommandHandler(IUserRepository repository, IUserContextService userContext, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _repository.GetByIdAsync(_userContext.UserId, cancellationToken);
            
        var targetUser = await _repository.GetByIdAsync(request.TargetUserId, cancellationToken);

        if (currentUser == null || targetUser == null)
            return Result.Failure(UserErrors.NotFound);

        var result = currentUser.Follow(targetUser);
        
        if (result.IsSuccess)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}

