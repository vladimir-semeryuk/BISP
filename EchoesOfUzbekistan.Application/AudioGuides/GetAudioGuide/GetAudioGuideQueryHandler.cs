using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.AudioGuides.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Guides;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
internal class GetAudioGuideQueryHandler : IQueryHandler<GetAudioGuideQuery, AudioGuideResponse>
{
    private readonly IGuideReadRepository _repository;
    private readonly IFileService _fileService;

    public GetAudioGuideQueryHandler(IGuideReadRepository repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task<Result<AudioGuideResponse>> Handle(GetAudioGuideQuery request, CancellationToken cancellationToken)
    {
        var guide = await _repository.GetByIdAsync(request.AudioGuideId, cancellationToken);

        if (guide == null)
            return Result.Failure<AudioGuideResponse>(AudioGuideErrors.NotFound);

        if (!string.IsNullOrWhiteSpace(guide.ImageKey))
        {
            guide.ImageLink = await _fileService.GetPresignedUrlForGetAsync(guide.ImageKey, cancellationToken);
        }
        if (!string.IsNullOrWhiteSpace(guide.AudioKey))
        {
            guide.AudioLink = await _fileService.GetPresignedUrlForGetAsync(guide.AudioKey, cancellationToken);
        }

        if (guide.Places.Count > 0)
        {
            foreach (var placeResponse in guide.Places)
            {
                if (placeResponse.ImageKey != null)
                    placeResponse.ImageLink = await _fileService.GetPresignedUrlForGetAsync(placeResponse.ImageKey);
                if (placeResponse.AudioKey != null)
                    placeResponse.AudioLink = await _fileService.GetPresignedUrlForGetAsync(placeResponse.AudioKey);
            }
        }

        return Result.Success(guide);
    }
}
