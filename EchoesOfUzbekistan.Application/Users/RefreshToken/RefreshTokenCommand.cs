using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.LoginUser;

namespace EchoesOfUzbekistan.Application.Users.RefreshToken;
public record RefreshTokenCommand(string RefreshToken) : ICommand<TokenResponse>;

