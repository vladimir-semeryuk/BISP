using EchoesOfUzbekistan.Domain.Places;
using Microsoft.EntityFrameworkCore;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Places;
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
            .Include(place => place.Guides)
            .Include(place => place.Translations)
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Place>> GetPlacesByIdsAsync(List<Guid> placeIds) =>
       await _context.Set<Place>().Where(p => placeIds.Contains(p.Id)).ToListAsync();
}
