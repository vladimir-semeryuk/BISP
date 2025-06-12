namespace EchoesOfUzbekistan.Domain.Users;
public class Role
{
    public static readonly Role OrdinaryUser = new(1, "OrdinaryUser");
    public static readonly Role Moderator = new(2, "Moderator");
    public static readonly Role Administrator = new(3, "Administrator");

    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public ICollection<User> Users { get; init; } = new List<User>();
}