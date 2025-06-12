using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Users.LoginUser;
public record LoginUserCommand(string Email, string Password) : ICommand<TokenResponse>;
