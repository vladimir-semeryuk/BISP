using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Places.GetPlace;

namespace EchoesOfUzbekistan.Application.Places.GetPlaces;

public record GetPlacesQuery(PlaceFilter Filter) : IQuery<PaginatedResponse<PlaceResponse>>;
