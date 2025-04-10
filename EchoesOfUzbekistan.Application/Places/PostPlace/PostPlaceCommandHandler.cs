using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.AudioGuides.PostAudioGuide;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Places;
using EchoesOfUzbekistan.Domain.Users;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Places.PostPlace;
public class PostPlaceCommandHandler : ICommandHandler<PostPlaceCommand, Guid>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILanguageRepository _languageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContextService _userContext;

    public PostPlaceCommandHandler(ILanguageRepository languageRepository, IUserContextService userContext, IUserRepository userRepository, IPlaceRepository placeRepository, IUnitOfWork unitOfWork)
    {
        _languageRepository = languageRepository;
        _userContext = userContext;
        _userRepository = userRepository;
        _placeRepository = placeRepository;
        _unitOfWork = unitOfWork;
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
            authorId: request.AuthorId);

        _placeRepository.Add(place);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(place.Id);
    }
}
