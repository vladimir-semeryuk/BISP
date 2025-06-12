using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Guides.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.AudioGuides;
internal class AudioGuideRepository : Repository<AudioGuide>, IGuideRepository
{
    public AudioGuideRepository(AppDbContext context) : base(context)
    {
    }

    public void Delete(AudioGuide guide)
    {
        _context.Set<AudioGuide>().Remove(guide);
    }

    public async Task<IEnumerable<AudioGuide>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context
            .Set<AudioGuide>()
            .Include(guide => guide.Places)
            .Include(guide => guide.Translations).ToListAsync();
    }

    public async Task<IList<AudioGuide>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        return await _context.Set<AudioGuide>()
            .Where(g => ids.Contains(g.Id))
            .Include(g => g.Places)
            .Include(g => g.Translations)
            .ToListAsync(cancellationToken);
    }

    public void Update(AudioGuide guide)
    {
        _context.Set<AudioGuide>().Update(guide);
    }

    public new async Task<AudioGuide?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context
            .Set<AudioGuide>()
            .Include(guide => guide.Places)
            .Include(guide => guide.Translations)
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }
}
