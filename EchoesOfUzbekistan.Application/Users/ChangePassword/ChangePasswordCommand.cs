using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Users.ChangePassword;

public record ChangePasswordCommand(string Email, string OldPassword, string NewPassword) : ICommand;
