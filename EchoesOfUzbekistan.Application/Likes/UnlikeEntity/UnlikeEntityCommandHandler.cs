using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Likes;
using FluentValidation;

namespace EchoesOfUzbekistan.Application.Likes.UnlikeEntity;
public class UnlikeEntityCommandHandler : ICommandHandler<UnlikeEntityCommand>
{
    private readonly IUserContextService _userContext;
    private readonly ILikeRepository _likeRepository;
    private readonly IValidator<UnlikeEntityCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UnlikeEntityCommandHandler(ILikeRepository likeRepository, IUserContextService userContext, IValidator<UnlikeEntityCommand> validator, IUnitOfWork unitOfWork)
    {
        _likeRepository = likeRepository;
        _userContext = userContext;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UnlikeEntityCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var userId = _userContext.UserId;

        var like = await _likeRepository.GetAsync(userId, request.EntityId, EntityTypeNames.AudioGuide);
        if (like == null)
            return Result.Failure(LikeErrors.NotFound);

        _likeRepository.Remove(like);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
