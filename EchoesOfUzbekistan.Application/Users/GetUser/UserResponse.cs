namespace EchoesOfUzbekistan.Application.Users.GetUser;
public class UserResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
    public DateTime RegistrationDateUtc { get; init; }
    public string CountryName { get; init; }
    public string CountryCode { get; init; }
    public string? City { get; init; }
    public string? AboutMe { get; init; }
    public string? ImageLink { get; set; }
    public string? ImageKey { get; init; }
    public int FriendCount { get; init; }
    public string[] Roles { get; set; } = Array.Empty<string>();
}
