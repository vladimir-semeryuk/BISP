using Dapper;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Likes.GetLikedEntities;
using EchoesOfUzbekistan.Application.Likes.Interfaces;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Likes;
public class LikeReadRepository : ILikeReadRepository
{
    private readonly ISQLConnectionFactory _connectionFactory;

    public LikeReadRepository(ISQLConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> GetLikeCountAsync(Guid entityId, string entityType, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT COUNT(*) 
            FROM likes 
            WHERE entity_id = @EntityId AND entity_type = @EntityType";

        using var connection = _connectionFactory.GetDbConnection();
        return await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            sql,
            new { EntityId = entityId, EntityType = entityType },
            cancellationToken: cancellationToken));
    }

    public async Task<bool> HasUserLikedAsync(Guid userId, Guid entityId, string entityType, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT COUNT(1)
            FROM likes 
            WHERE user_id = @UserId AND entity_id = @EntityId AND entity_type = @EntityType";

        using var connection = _connectionFactory.GetDbConnection();
        var result = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            sql,
            new { UserId = userId, EntityId = entityId, EntityType = entityType },
            cancellationToken: cancellationToken));

        return result > 0;
    }

    public async Task<IEnumerable<LikedAudioGuideDto>> GetLikedAudioGuidesAsync(Guid userId, int pageNumber, int pageSize)
    {
        const string sql = @"
            SELECT ag.id AS Id, ag.title AS Title, ag.city AS City, ag.date_published AS DatePublished, ag.image_link AS ImageLink
            FROM likes l
            JOIN audio_guides ag ON ag.id = l.entity_id
            WHERE l.user_id = @UserId AND l.entity_type = 'AudioGuide'
            LIMIT @Limit OFFSET @Offset";

        using var connection = _connectionFactory.GetDbConnection();
        return await connection.QueryAsync<LikedAudioGuideDto>(sql, new
        {
            UserId = userId,
            Limit = pageSize,
            Offset = (pageNumber - 1) * pageSize
        });
    }
}

