using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Abstractions.Auth;
public interface IAuthService
{
    Task<string> Signup(
        User user, 
        string password,
        CancellationToken cancellationToken = default);

    Task ChangePasswordAsync(
        string userId,
        string newPassword,
        CancellationToken cancellationToken);
}
