using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.LoginUser;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Users.RefreshToken;
public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await _tokenService.RefreshAccessTokenAsync(request.RefreshToken, cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<TokenResponse>(UserErrors.InvalidRefreshToken);
        }

        return result;
    }
}
