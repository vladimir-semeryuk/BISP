using Dapper;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;
internal class GetAudioGuidesQueryHandler : IQueryHandler<GetAudioGuidesQuery, IReadOnlyList<AudioGuideShortResponse>>
{
    private readonly ISQLConnectionFactory _connectionFactory;

    public GetAudioGuidesQueryHandler(ISQLConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<Result<IReadOnlyList<AudioGuideShortResponse>>> Handle(GetAudioGuidesQuery request, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.GetDbConnection();
        var sqlBuilder = new StringBuilder(@"
        SELECT 
            ag.id AS Id,
            ag.title AS Title, 
            ag.description AS Description, 
            ag.city AS City, 
            ag.price_amount AS PriceAmount, 
            ag.price_currency AS PriceCurrency, 
            ag.status AS Status, 
            ag.date_published AS DatePublished, 
            ag.date_edited AS DateEdited, 
            ag.author_id AS AuthorId, 
            ag.original_language_id AS OriginalLanguageId, 
            ag.audio_link AS AudioLink, 
            ag.image_link AS ImageLink
        FROM audio_guides ag
        WHERE 1=1 ");

        var parameters = new DynamicParameters();

        if (request.Filter.CreatedByUserId is not null)
        {
            sqlBuilder.Append(" AND ag.author_id = @CreatedByUserId");
            parameters.Add("CreatedByUserId", request.Filter.CreatedByUserId);
        }

        if (request.Filter.GetNewest)
        {
            sqlBuilder.Append(" ORDER BY ag.date_published DESC");
        }
        else
        {
            sqlBuilder.Append(" ORDER BY ag.title ASC");
        }

        if (request.Filter.GetTopN.HasValue)
        {
            sqlBuilder.Append(" LIMIT @Limit");
            parameters.Add("Limit", request.Filter.GetTopN.Value);
        }

        var sql = sqlBuilder.ToString();
        IEnumerable<AudioGuideShortResponse> result = await connection.QueryAsync<AudioGuideShortResponse>(sql, parameters);
        return result.ToList();
    }
}
