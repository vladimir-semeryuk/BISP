using EchoesOfUzbekistan.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Authorisation;
internal class MyClaimsTransformation : IClaimsTransformation
{
    private readonly IServiceProvider _serviceProvider;

    public MyClaimsTransformation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not { IsAuthenticated: true } ||
            principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
            principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
        {
            return principal;
        }
        using IServiceScope scope = _serviceProvider.CreateScope();

        AuthorisationService authorisationService = scope.ServiceProvider.GetRequiredService<AuthorisationService>();

        string identityId = principal.GetIdentityId();

        UserIdRolesResponse userRoles = await authorisationService.GetRolesForUser(identityId);

        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.UserId.ToString()));

        foreach (Role role in userRoles.Roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        }

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}

