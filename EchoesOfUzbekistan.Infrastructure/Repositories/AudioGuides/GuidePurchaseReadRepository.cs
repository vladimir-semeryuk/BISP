using Dapper;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.AudioGuides.Interfaces;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.AudioGuides;
internal class GuidePurchaseReadRepository : IGuidePurchaseReadRepository
{
    private readonly ISQLConnectionFactory _connectionFactory;

    public GuidePurchaseReadRepository(ISQLConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid guideId)
    {
        using var connection = _connectionFactory.GetDbConnection();

        const string sql = @"
            SELECT 1
            FROM audio_guide_purchases
            WHERE user_id = @UserId AND guide_id = @GuideId
            LIMIT 1;";

        var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { UserId = userId, GuideId = guideId });

        return result.HasValue;
    }

    public async Task<List<Guid>> GetPurchasedGuideIdsAsync(Guid userId)
    {
        using var connection = _connectionFactory.GetDbConnection();

        const string sql = @"
            SELECT guide_id
            FROM audio_guide_purchases
            WHERE user_id = @UserId;";

        var guideIds = await connection.QueryAsync<Guid>(sql, new { UserId = userId });

        return guideIds.ToList();
    }
}
