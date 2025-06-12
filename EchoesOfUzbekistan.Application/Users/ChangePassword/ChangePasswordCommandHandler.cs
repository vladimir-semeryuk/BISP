using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Users.ChangePassword;
internal class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IUserContextService _userContextService;
    private readonly ITokenService _tokenService;
    public ChangePasswordCommandHandler(IUserRepository userRepository, IAuthService authService, IUserContextService userContextService, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _authService = authService;
        _userContextService = userContextService;
        _tokenService = tokenService;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (user.Id != _userContextService.UserId)
            return Result.Failure(Error.UnauthorizedAccess);

        var tokenResult = await _tokenService.GetAccessTokenAsync(request.Email, request.OldPassword, cancellationToken);
        if (tokenResult.IsFailure)
        {
            return Result.Failure(UserErrors.InvalidCredentials);
        }

        try
        {
            await _authService.ChangePasswordAsync(user.IdentityId, request.NewPassword, cancellationToken);
        }
        catch (Exception e)
        {
            return Result.Failure(UserErrors.InvalidRefreshToken);
        }

        return Result.Success();
    }
}
