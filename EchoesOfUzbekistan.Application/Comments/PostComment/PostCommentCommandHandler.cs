using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Comments;
using FluentValidation;

namespace EchoesOfUzbekistan.Application.Comments.PostComment;
internal class PostCommentCommandHandler : ICommandHandler<PostCommentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;
    private readonly ICommentRepository _commentRepository;
    // private readonly IValidator<PostCommentCommand> _validator;

    public PostCommentCommandHandler(IUnitOfWork unitOfWork, ICommentRepository commentRepository, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        // _validator = validator;
        _commentRepository = commentRepository;
        _userContextService = userContextService;
    }

    public async Task<Result<Guid>> Handle(PostCommentCommand request, CancellationToken cancellationToken)
    {
        // var validation = await _validator.ValidateAsync(request, cancellationToken);
        // if (!validation.IsValid)
        //     throw new ValidationException(validation.Errors);

        var userId = _userContextService.UserId;

        var comment = new Comment(
            userId,
            request.EntityId,
            request.EntityType,
            request.Content.Trim());

        _commentRepository.Add(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }
}
