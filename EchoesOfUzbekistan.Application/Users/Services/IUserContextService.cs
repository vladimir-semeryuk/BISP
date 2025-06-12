namespace EchoesOfUzbekistan.Application.Users.Services;
public interface IUserContextService
{
    Guid UserId { get; }
    string IdentityId { get; }

    List<string> Roles { get; }
}
