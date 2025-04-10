using EchoesOfUzbekistan.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Repositories;
internal class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<User>().ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<User?> GetByIdentityIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(user => user.IdentityId == id, cancellationToken: cancellationToken);
    }

    public override void Add(User entity)
    {
        foreach (Role role in entity.Roles)
        {
            _context.Attach(role);
        }

        _context.Add(entity);
    }
}
