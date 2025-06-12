using Dapper;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Comments.GetCommentsForEntity;
using EchoesOfUzbekistan.Application.Comments.Interfaces;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Comments;
internal class CommentReadRepository : ICommentReadRepository
{
    private readonly ISQLConnectionFactory _connectionFactory;

    public CommentReadRepository(ISQLConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<CommentForEntityDto>> GetCommentsForEntityAsync(Guid entityId, string entityType, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                c.id,
                c.content,
                c.entity_id AS EntityId,
                c.entity_type AS EntityType,
                c.user_id AS AuthorId,
                CONCAT(u.first_name, ' ', u.surname) AS AuthorName,
                u.image_link AS AuthorAvatar,
                c.created_at AS DateCreated
            FROM comments c
            JOIN users u ON c.user_id = u.id
            WHERE c.entity_id = @EntityId AND c.entity_type = @EntityType
            ORDER BY c.created_at DESC";

        using var connection = _connectionFactory.GetDbConnection();
        var result = await connection.QueryAsync<CommentForEntityDto>(sql, new { EntityId = entityId, EntityType = entityType });


        return result.ToList();
    }
}
