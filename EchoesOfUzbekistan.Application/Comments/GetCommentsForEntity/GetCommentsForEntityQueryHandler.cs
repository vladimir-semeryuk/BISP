using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Comments.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Comments.GetCommentsForEntity;
public class GetCommentsForEntityQueryHandler : IQueryHandler<GetCommentsForEntityQuery, List<CommentForEntityDto>>
{
    private readonly ICommentReadRepository _commentRepository;
    private readonly IFileService _fileService;

    public GetCommentsForEntityQueryHandler(ICommentReadRepository commentRepository, IFileService fileService)
    {
        _commentRepository = commentRepository;
        _fileService = fileService;
    }

    public async Task<Result<List<CommentForEntityDto>>> Handle(GetCommentsForEntityQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetCommentsForEntityAsync(request.EntityId, request.EntityType, cancellationToken);

        if (comments == null || comments.Count == 0)
            return comments;

        foreach (CommentForEntityDto comment in comments)
        {
            if (string.IsNullOrEmpty(comment.AuthorAvatar))
                continue;

            comment.AuthorAvatar =
                await _fileService.GetPresignedUrlForGetAsync(comment.AuthorAvatar, cancellationToken);
        }

        return comments;
    }
}
