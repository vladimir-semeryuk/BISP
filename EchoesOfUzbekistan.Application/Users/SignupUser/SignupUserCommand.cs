using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Users.SignupUser;
public record SignupUserCommand(
    string Email,
    string FirstName,
    string Surname,
    string Password,
    string CountryName,
    string CountryCode,
    string? City) : ICommand<Guid>;
