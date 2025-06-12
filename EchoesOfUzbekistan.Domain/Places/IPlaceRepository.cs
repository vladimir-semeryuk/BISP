namespace EchoesOfUzbekistan.Domain.Places;
public interface IPlaceRepository
{
    Task<Place?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Place>> GetPlacesByIdsAsync(List<Guid> placeIds);
    Task<IEnumerable<Place>> GetAllAsync(CancellationToken cancellationToken);
    void Add(Place place);
    void Update(Place place);
    void Delete(Place place);
}
