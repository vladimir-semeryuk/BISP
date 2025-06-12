using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Users.UpdateUser;
public record UpdateUserCommand(
    Guid UserId, 
    string? FirstName, 
    string? Surname, 
    string? AboutMe, 
    string? CountryCode,
    string? CountryName,
    string? City, 
    string? ImageLink) : ICommand;