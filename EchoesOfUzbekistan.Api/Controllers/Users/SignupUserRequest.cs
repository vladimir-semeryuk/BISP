namespace EchoesOfUzbekistan.Api.Controllers.Users;

public record SignupUserRequest(
    string Email, 
    string FirstName, 
    string Surname, 
    string Password, 
    string CountryCode,
    string CountryName);
