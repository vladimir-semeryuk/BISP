namespace EchoesOfUzbekistan.Application.Users.GetUsers;

public class UserFilter
{
    public string? SearchTerm { get; set; } = null;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}