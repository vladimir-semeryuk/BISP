using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Places.GetPlace;
public record GetPlaceQuery(Guid PlaceId) : IQuery<PlaceDetailsResponse>;