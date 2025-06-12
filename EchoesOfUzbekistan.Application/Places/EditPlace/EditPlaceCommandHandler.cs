using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides.Repositories;
using EchoesOfUzbekistan.Domain.Places;
using NetTopologySuite.Geometries;

namespace EchoesOfUzbekistan.Application.Places.EditPlace;
public class EditPlaceCommandHandler : ICommandHandler<EditPlaceCommand>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;
    private readonly ILanguageRepository _languageRepository;
    private readonly IGuideRepository _guideRepository; 

    public EditPlaceCommandHandler(IPlaceRepository placeRepository, IUnitOfWork unitOfWork, IUserContextService userContextService, ILanguageRepository languageRepository, IGuideRepository guideRepository)
    {
        _placeRepository = placeRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
        _languageRepository = languageRepository;
        _guideRepository = guideRepository;
    }

    public async Task<Result> Handle(EditPlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _placeRepository.GetByIdAsync(request.PlaceId, cancellationToken);
        if (place is null)
            return Result.Failure(PlaceErrors.NotFound);

        var hasAccess = AuthorizationGuard.EnsureUserOwnsResource(_userContextService.UserId, place.AuthorId);
        if (hasAccess.IsFailure && AuthorizationGuard.EnsureUserIsAdminModerator(_userContextService).IsFailure)
            return Result.Failure(Error.CannotPostForOthers);

        var language = await _languageRepository.GetLanguageByCode(request.LanguageCode, cancellationToken);
        if (language == null)
            return Result.Failure(LanguageErrors.NotFoundCode);

        try
        {
            place.Edit(
                new PlaceTitle(request.Title),
                string.IsNullOrWhiteSpace(request.Description) ? null : PlaceDescription.Create(request.Description),
                new Point(request.Longitude, request.Latitude) { SRID = 4326 },
                language.Id,
                request.AudioLink == null ? null : new ResourceLink(request.AudioLink),
                request.ImageLink == null ? null : new ResourceLink(request.ImageLink)
            );
        }
        catch (ArgumentException ex)
        {
            var error = new Error("Validation.Failed", ex.Message);
            return Result.Failure(error);
        }

        var targetGuideIds = request.AudioGuidesIds?.Distinct().ToList() ?? new List<Guid>();

        if (targetGuideIds.Count == 0)
        {
            // Detach from all existing guides
            foreach (var guide in place.Guides.ToList())
            {
                guide.RemovePlace(place);
            }
        }
        else
        {
            // Fetch only the target guides
            var allTargetGuides = await _guideRepository.GetByIdsAsync(targetGuideIds, cancellationToken);

            // Remove from guides that are no longer in the list
            foreach (var guide in place.Guides.ToList())
            {
                if (!targetGuideIds.Contains(guide.Id))
                {
                    guide.RemovePlace(place);
                }
            }

            // Add to new guides
            foreach (var guide in allTargetGuides)
            {
                if (!guide.Places.Contains(place))
                {
                    guide.AddPlace(place);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
