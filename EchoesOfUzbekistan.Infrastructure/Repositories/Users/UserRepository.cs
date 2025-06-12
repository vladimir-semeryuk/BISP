using EchoesOfUzbekistan.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Users;
internal class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<User>().ToListAsync(cancellationToken: cancellationToken);
    }

    public new async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .Include(u => u.Followers)
            .Include(u => u.Following)
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var userEmail = new Email(email);
        var result = await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == userEmail, cancellationToken);
        return result;
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

    public void Delete(User user)
    {
        _context.Set<User>().Remove(user);
    }
}
