using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Users.DeleteUser;
internal class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserContextService _userContextService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserContextService userContextService, IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _userContextService = userContextService;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var authResult = AuthorizationGuard.EnsureUserIsAdmin(_userContextService);
        if (authResult.IsFailure)
            return authResult;

        var userToDelete = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (userToDelete == null)
            return Result.Failure(UserErrors.NotFound);

        userToDelete.Delete();
        _userRepository.Delete(userToDelete);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
