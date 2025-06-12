using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Places.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Places;

namespace EchoesOfUzbekistan.Application.Places.GetPlace;
internal class GetPlaceQueryHandler : IQueryHandler<GetPlaceQuery, PlaceDetailsResponse>
{
    private readonly IPlaceReadRepository _placeReadRepository;
    private readonly IFileService _fileService; 

    public GetPlaceQueryHandler(IPlaceReadRepository placeReadRepository, IFileService fileService)
    {
        _placeReadRepository = placeReadRepository;
        _fileService = fileService;
    }

    public async Task<Result<PlaceDetailsResponse>> Handle(GetPlaceQuery request, CancellationToken cancellationToken)
    {
        var place = await _placeReadRepository.GetByIdAsync(request.PlaceId, cancellationToken);

        if (place == null)
        {
            return Result.Failure<PlaceDetailsResponse>(PlaceErrors.NotFound);
        }

        if (place.ImageKey != null)
        {
            place.ImageLink = await _fileService.GetPresignedUrlForGetAsync(place.ImageKey, cancellationToken);
        }

        if (place.AudioKey != null)
        {
            place.AudioLink = await _fileService.GetPresignedUrlForGetAsync(place.AudioKey, cancellationToken);
        }

        return place;
    }
}
