using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides.Repositories;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Users;
using NetTopologySuite.Geometries;

namespace EchoesOfUzbekistan.Application.Places.PostPlace;
public class PostPlaceCommandHandler : ICommandHandler<PostPlaceCommand, Guid>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILanguageRepository _languageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContextService _userContext;
    private readonly IGuideRepository _guideRepository;

    public PostPlaceCommandHandler(ILanguageRepository languageRepository, IUserContextService userContext, IUserRepository userRepository, IPlaceRepository placeRepository, IUnitOfWork unitOfWork, IGuideRepository guideRepository)
    {
        _languageRepository = languageRepository;
        _userContext = userContext;
        _userRepository = userRepository;
        _placeRepository = placeRepository;
        _unitOfWork = unitOfWork;
        _guideRepository = guideRepository;
    }

    public async Task<Result<Guid>> Handle(PostPlaceCommand request, CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetLanguageByCode(request.LanguageCode, cancellationToken);
        if (language == null)
        {
            return Result.Failure<Guid>(LanguageErrors.NotFound);
        }
        if (request.AuthorId != _userContext.UserId)
        {
            return Result.Failure<Guid>(UserErrors.CannotPostForOthers);
        }

        var author = await _userRepository.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author == null)
        {
            return Result.Failure<Guid>(UserErrors.AuthorNotFound);
        }

        var point = new Point(request.Longitude, request.Latitude);

        var place = new Place(
            id: Guid.NewGuid(),
            title: new PlaceTitle(request.Title),
            description: new PlaceDescription(request.Description),
            coordinates: point,
            status: PlaceStatus.Visible,
            originalLanguageId: language.Id,
            authorId: request.AuthorId,
            audioLink: request.AudioLink == null ? null : new ResourceLink(request.AudioLink),
            imageLink: request.ImageLink == null ? null : new ResourceLink(request.ImageLink));

        var targetGuideIds = request.AudioGuidesIds?.Distinct().ToList() ?? new List<Guid>();

        if (targetGuideIds.Count != 0)
        {
            var allTargetGuides = await _guideRepository.GetByIdsAsync(targetGuideIds, cancellationToken);
            // Add to new guides
            foreach (var guide in allTargetGuides)
            {
                if (!guide.Places.Contains(place))
                {
                    guide.AddPlace(place);
                }
            }
        }
        

        _placeRepository.Add(place);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(place.Id);
    }
}
