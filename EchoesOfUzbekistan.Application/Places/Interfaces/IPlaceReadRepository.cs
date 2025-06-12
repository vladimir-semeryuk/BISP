using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Places.GetPlace;
using EchoesOfUzbekistan.Application.Places.GetPlaces;

namespace EchoesOfUzbekistan.Application.Places.Interfaces;
public interface IPlaceReadRepository
{
    Task<PaginatedResponse<PlaceResponse>> GetPlacesAsync(PlaceFilter filter);
    Task<PlaceDetailsResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
