using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Application.AudioGuides.Interfaces;
using EchoesOfUzbekistan.Application.Abstractions.Data;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;
internal class GetAudioGuidesQueryHandler : IQueryHandler<GetAudioGuidesQuery, PaginatedResponse<AudioGuideShortResponse>>
{
    private readonly IGuideReadRepository _audioGuideRepository;
    private readonly IFileService _fileService;

    public GetAudioGuidesQueryHandler(IGuideReadRepository audioGuideRepository, IFileService fileService)
    {
        _audioGuideRepository = audioGuideRepository;
        _fileService = fileService;
    }

    public async Task<Result<PaginatedResponse<AudioGuideShortResponse>>> Handle(GetAudioGuidesQuery request, CancellationToken cancellationToken)
    {
        var result = await _audioGuideRepository.GetAudioGuidesAsync(request.Filter);

        // Add URLs to the response
        foreach (var guide in result.Items)
        {
            if (!string.IsNullOrWhiteSpace(guide.ImageKey))
            {
                guide.ImageLink = await _fileService.GetPresignedUrlForGetAsync(guide.ImageKey, cancellationToken);
            }
            if (!string.IsNullOrWhiteSpace(guide.AudioKey))
            {
                guide.AudioLink = await _fileService.GetPresignedUrlForGetAsync(guide.AudioKey, cancellationToken);
            }
        }

        return result;
    }
}
