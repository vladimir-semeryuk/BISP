using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Users.LoginUser;
internal class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, TokenResponse>
{
    private readonly ITokenService _tokenService;
    public LoginUserCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<Result<TokenResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        Result<TokenResponse> result = await _tokenService.GetAccessTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<TokenResponse>(result.Error);
        }

        return result;
    }
}
