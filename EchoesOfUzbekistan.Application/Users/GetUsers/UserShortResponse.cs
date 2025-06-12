namespace EchoesOfUzbekistan.Application.Users.GetUsers;
public class UserShortResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
    public string? AboutMe { get; init; }
    public string? ImageLink { get; set; }
    public string? ImageKey { get; init; }
}
