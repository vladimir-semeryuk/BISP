using EchoesOfUzbekistan.Application.Users.LoginUser;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Abstractions.Auth;
public interface ITokenService
{
    Task<Result<TokenResponse>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<Result<TokenResponse>> RefreshAccessTokenAsync(string refreshToken,
        CancellationToken cancellationToken = default);
}
