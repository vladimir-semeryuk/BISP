using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Guides.Repositories;
using EchoesOfUzbekistan.Domain.Likes;
using FluentValidation;

namespace EchoesOfUzbekistan.Application.Likes.LikeEntity;
public class LikeEntityCommandHandler : ICommandHandler<LikeEntityCommand>
{
    private readonly IUserContextService _userContext;
    private readonly IGuideRepository _guideRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<LikeEntityCommand> _validator;

    public LikeEntityCommandHandler(ILikeRepository likeRepository, IGuideRepository guideRepository, IUserContextService userContext, IValidator<LikeEntityCommand> validator, IUnitOfWork unitOfWork)
    {
        _likeRepository = likeRepository;
        _guideRepository = guideRepository;
        _userContext = userContext;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LikeEntityCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        if (request.EntityType == EntityTypeNames.AudioGuide)
        {
            var audioGuide = await _guideRepository.GetByIdAsync(request.EntityId, cancellationToken);
            if (audioGuide == null)
                return Result.Failure(AudioGuideErrors.NotFound);
        }

        var userId = _userContext.UserId;

        var likeExists = await _likeRepository.ExistsAsync(userId, request.EntityId, EntityTypeNames.AudioGuide);
        if (likeExists)
            return Result.Failure(LikeErrors.AlreadyLiked);

        var like = new Like(userId, request.EntityId, EntityTypeNames.AudioGuide);
        _likeRepository.Add(like);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
