using EchoesOfUzbekistan.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Authorisation;
internal class AuthorisationService
{
    private readonly AppDbContext _appDbContext;

    public AuthorisationService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<UserIdRolesResponse> GetRolesForUser(string identityId)
    {
        var userRoles = await _appDbContext.Set<User>()
                                            .Where(u => u.IdentityId == identityId)
                                            .Select(u => new UserIdRolesResponse
                                            {
                                                UserId = u.Id,
                                                Roles = u.Roles.ToList()
                                            })
                                            .FirstOrDefaultAsync();
        return userRoles;
    }
}

