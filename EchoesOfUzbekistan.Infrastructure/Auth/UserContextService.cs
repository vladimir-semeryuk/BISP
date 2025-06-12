using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Infrastructure.Authorisation;
using Microsoft.AspNetCore.Http;

namespace EchoesOfUzbekistan.Infrastructure.Auth;
public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User Id Helper could not find the user id");

    public string IdentityId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetIdentityId() ??
        throw new ApplicationException("User Id Helper could not find the user id");

    public List<string> Roles =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetRoles() ??
        throw new ApplicationException("User Id Helper could not find the user roles");
}
