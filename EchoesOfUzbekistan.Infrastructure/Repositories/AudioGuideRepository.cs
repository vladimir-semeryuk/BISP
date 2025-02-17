using EchoesOfUzbekistan.Domain.Guides;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Repositories;
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
