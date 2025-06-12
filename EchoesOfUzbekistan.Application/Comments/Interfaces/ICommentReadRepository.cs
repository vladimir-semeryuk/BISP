using EchoesOfUzbekistan.Application.Comments.GetCommentsForEntity;

namespace EchoesOfUzbekistan.Application.Comments.Interfaces;
public interface ICommentReadRepository
{
    Task<List<CommentForEntityDto>> GetCommentsForEntityAsync(Guid entityId, string entityType, CancellationToken cancellationToken);
}

