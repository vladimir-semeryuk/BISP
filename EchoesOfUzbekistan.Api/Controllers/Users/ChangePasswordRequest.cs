namespace EchoesOfUzbekistan.Api.Controllers.Users;

public record ChangePasswordRequest(string Email, string OldPassword, string NewPassword);