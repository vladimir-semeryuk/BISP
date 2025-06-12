using EchoesOfUzbekistan.Domain.Likes;
using Microsoft.EntityFrameworkCore;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Likes;
internal class LikeRepository : Repository<Like>, ILikeRepository
{
    public LikeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid entityId, string entityType)
    {
        return await _context.Set<Like>().AnyAsync(l =>
            l.UserId == userId &&
            l.EntityId == entityId &&
            l.EntityType == entityType);
    }

    public async Task<Like?> GetAsync(Guid userId, Guid entityId, string entityType)
    {
        return await _context.Set<Like>().FirstOrDefaultAsync(l =>
            l.UserId == userId &&
            l.EntityId == entityId &&
            l.EntityType == entityType);
    }

    public void Remove(Like like)
    {
        _context.Set<Like>().Remove(like);
    }
}
