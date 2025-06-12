namespace EchoesOfUzbekistan.Application.Users.GetFollowers;
public class FriendResponse
{
    public Guid FriendId { get; init; }
    public string FirstName { get; init; }
    public string Surname { get; init; }
    public string? ImageLink { get; set; }
}
