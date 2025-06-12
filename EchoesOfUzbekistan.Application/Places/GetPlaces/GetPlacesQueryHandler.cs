using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Places.GetPlace;
using EchoesOfUzbekistan.Application.Places.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Places.GetPlaces;
internal class GetPlacesQueryHandler : IQueryHandler<GetPlacesQuery, PaginatedResponse<PlaceResponse>>
{
    private readonly IPlaceReadRepository _placeReadRepository;
    private readonly IFileService _fileService;

    public GetPlacesQueryHandler(IPlaceReadRepository placeReadRepository, IFileService fileService)
    {
        _placeReadRepository = placeReadRepository;
        _fileService = fileService;
    }

    public async Task<Result<PaginatedResponse<PlaceResponse>>> Handle(GetPlacesQuery request, CancellationToken cancellationToken)
    {
        var result = await _placeReadRepository.GetPlacesAsync(request.Filter);

        if (result == null)
            return Result.Success(new PaginatedResponse<PlaceResponse>());

        var places = result.Items;

        foreach (var place in places)
        {
            if (!string.IsNullOrWhiteSpace(place.ImageKey))
            {
                place.ImageLink = await _fileService.GetPresignedUrlForGetAsync(place.ImageKey, cancellationToken);
            }
        }

        return result;
    }
}
