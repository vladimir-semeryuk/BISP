using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Places;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Repositories;
internal class PlaceRepository : Repository<Place>, IPlaceRepository
{
    public PlaceRepository(AppDbContext context) : base(context)
    {
    }

    public void Delete(Place place)
    {
        _context.Set<Place>().Remove(place);
    }

    public async Task<IEnumerable<Place>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context
            .Set<Place>()
            .Include(place => place.Guides)
            .Include(place => place.Translations).ToListAsync();
    }

    public void Update(Place place)
    {
        _context.Set<Place>().Update(place);
    }
    public new async Task<Place?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context
            .Set<Place>()
            .Include(guide => guide.Translations)
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }
}
