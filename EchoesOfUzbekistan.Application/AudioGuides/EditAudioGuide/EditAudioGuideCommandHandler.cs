using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Guides.Repositories;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.AudioGuides.EditAudioGuide;
internal class EditAudioGuideCommandHandler : ICommandHandler<EditAudioGuideCommand>
{
    private readonly IGuideRepository _audioGuideRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IUserContextService _userContextService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlaceRepository _placeRepository;

    public EditAudioGuideCommandHandler(
        IGuideRepository audioGuideRepository,
        ILanguageRepository languageRepository,
        IUnitOfWork unitOfWork, IUserContextService userContextService, IPlaceRepository placeRepository)
    {
        _audioGuideRepository = audioGuideRepository;
        _languageRepository = languageRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
        _placeRepository = placeRepository;
    }
    public async Task<Result> Handle(EditAudioGuideCommand request, CancellationToken cancellationToken)
    {
        var language = await _languageRepository.GetLanguageByCode(request.LanguageCode, cancellationToken);
        if (language == null)
            return Result.Failure<Guid>(LanguageErrors.NotFound);

        var audioGuide = await _audioGuideRepository.GetByIdAsync(request.GuideId, cancellationToken);
        if (audioGuide == null)
            return Result.Failure<Guid>(AudioGuideErrors.NotFound);

        var userId = _userContextService.UserId;
        var ownsContent = AuthorizationGuard.EnsureUserOwnsResource(userId, audioGuide.AuthorId);
        if (ownsContent.IsFailure && AuthorizationGuard.EnsureUserIsAdminModerator(_userContextService).IsFailure) 
            return Result.Failure(UserErrors.CannotPostForOthers);

        audioGuide.Update(
            title: new GuideTitle(request.Title),
            description: new GuideInfo(request.Description),
            city: new City(request.City),
            price: new Money(request.MoneyAmount, Currency.FromCode(request.CurrencyCode)),
            languageId: language.Id,
            guideStatus: GuideStatus.Active,
            newAudioLink: request.AudioLink != null ? new ResourceLink(request.AudioLink!) : null,
            newImageLink: request.ImageLink != null ? new ResourceLink(request.ImageLink!) : null);

        if (request.PlacesIds != null && request.PlacesIds.Any())
        {
            var places = await _placeRepository.GetPlacesByIdsAsync(request.PlacesIds);

            foreach (var place in places)
            {
                audioGuide.AddPlace(place);
            }
        }

        _audioGuideRepository.Update(audioGuide);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(audioGuide.Id);
    }
}
