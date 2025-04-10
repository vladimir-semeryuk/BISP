using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Guides.Repositories;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.PostAudioGuide;
internal class PostAudioGuideCommandHandler : ICommandHandler<PostAudioGuideCommand, Guid>
{
    private readonly IGuideRepository _audioGuideRepository;
    private readonly IPlaceRepository _placeRepository;
    private readonly ILanguageRepository _languageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public PostAudioGuideCommandHandler(
        IGuideRepository audioGuideRepository,
        ILanguageRepository languageRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPlaceRepository placeRepository,
        IUserContextService userContext)
    {
        _audioGuideRepository = audioGuideRepository;
        _languageRepository = languageRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _placeRepository = placeRepository;
        _userContext = userContext;
    }
    public async Task<Result<Guid>> Handle(PostAudioGuideCommand request, CancellationToken cancellationToken)
    {
        if (request.AuthorId == Guid.Empty)
        {
            return Result.Failure<Guid>(UserErrors.AuthorNotFound);
        }

        if (request.AuthorId != _userContext.UserId)
        {
            return Result.Failure<Guid>(UserErrors.AuthorNotFound); // CHANGE TO CANNOT POST FOR OTHER USERS
        }

        var language = await _languageRepository.GetLanguageByCode(request.LanguageCode, cancellationToken);
        if (language == null)
        {
            return Result.Failure<Guid>(LanguageErrors.NotFound);
        }

        var author = await _userRepository.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author == null)
        {
            return Result.Failure<Guid>(UserErrors.AuthorNotFound);
        }

        var audioGuide = AudioGuide.Create(title: new GuideTitle(request.Title),
            description: new GuideInfo(request.Description),
            city: new City(request.City),
            price: new Money(request.MoneyAmount, Currency.FromCode(request.CurrencyCode)),
            languageId: language.Id,
            authorId: request.AuthorId,
            audioLink: request.AudioLink != null ? new ResourceLink(request.AudioLink!) : null,
            imageLink: request.ImageLink != null ? new ResourceLink(request.ImageLink!) : null);

        if (request.PlaceIds != null && request.PlaceIds.Any())
        {
            var places = await _placeRepository.GetPlacesByIdsAsync(request.PlaceIds);

            foreach (var place in places)
            {
                audioGuide.AddPlace(place);
            }
        }

        _audioGuideRepository.Add(audioGuide);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(audioGuide.Id);
    }
}
