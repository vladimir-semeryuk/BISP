using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace EchoesOfUzbekistan.Infrastructure.Authorisation;
internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new ApplicationException("User id is unavailable");
    }

    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new ApplicationException("User identity is unavailable");
    }

    public static List<string> GetRoles(this ClaimsPrincipal? principal)
    {
        if (principal == null)
            throw new ApplicationException("User principal is unavailable");

        return principal.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
    }
}