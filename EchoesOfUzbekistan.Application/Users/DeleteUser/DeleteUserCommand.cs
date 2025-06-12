using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Users.DeleteUser;

public record DeleteUserCommand(Guid UserId) : ICommand;
