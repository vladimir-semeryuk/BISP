using EchoesOfUzbekistan.Domain.Comments;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Comments;
public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context)
    {
    }
}
