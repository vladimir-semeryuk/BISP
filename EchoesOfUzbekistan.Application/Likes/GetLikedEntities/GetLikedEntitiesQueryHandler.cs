using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Likes.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Likes.GetLikedEntities;
internal class GetLikedEntitiesQueryHandler : IQueryHandler<GetLikedEntitiesQuery, IEnumerable<LikedAudioGuideDto>>
{
    private readonly ILikeReadRepository _repository;
    private readonly IFileService _fileService;

    public GetLikedEntitiesQueryHandler(ILikeReadRepository repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task<Result<IEnumerable<LikedAudioGuideDto>>> Handle(GetLikedEntitiesQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetLikedAudioGuidesAsync(request.UserId, request.PageNumber, request.PageSize);

        if (!result.Any())
            return result.ToList();

        foreach (LikedAudioGuideDto guide in result)
        {
            guide.ImageLink = await _fileService.GetPresignedUrlForGetAsync(guide.ImageLink, cancellationToken);
        }

        return result.ToList();
    }
}
