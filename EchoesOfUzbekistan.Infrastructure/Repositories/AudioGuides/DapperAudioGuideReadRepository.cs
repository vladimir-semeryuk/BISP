using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.AudioGuides.Interfaces;
using Dapper;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
using EchoesOfUzbekistan.Application.Places.GetPlace;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.AudioGuides;
public class DapperAudioGuideReadRepository : IGuideReadRepository
{
    private readonly ISQLConnectionFactory _connectionFactory;

    public DapperAudioGuideReadRepository(ISQLConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<AudioGuideResponse?> GetByIdAsync(Guid audioGuideId, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.GetDbConnection();
        var sql = @"SELECT 
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
    u.first_name || ' ' || u.surname AS AuthorName,
    ag.original_language_id AS OriginalLanguageId, 
    ag.audio_link AS AudioKey, 
    ag.image_link AS ImageKey,
    l.code AS LanguageCode,

    -- Likes
    COALESCE(like_counts.like_count, 0) AS LikeCount,

    -- Translations
    gt.language_id AS TranslationLanguageId,
    gt.title AS Title,
    gt.description AS Description,
    gt.audio_link AS AudioKey,

    -- Places (joined through audio_guide_place)
    p.id AS PlaceId,
    p.title AS Title,
    p.description AS Description,
    ST_AsGeoJSON(p.coordinates) AS Coordinates,
    p.status AS Status,
    p.date_published AS DatePublished,
    p.date_edited AS DateEdited,
    p.author_id AS AuthorId,
    p.original_language_id AS OriginalLanguageId,
    p.audio_link AS AudioKey, 
    p.image_link AS ImageKey

FROM audio_guides ag

LEFT JOIN (
    SELECT entity_id, COUNT(*) AS like_count
    FROM likes
    WHERE entity_type = 'AudioGuide'
    GROUP BY entity_id
) like_counts ON like_counts.entity_id = ag.id

LEFT JOIN users u ON ag.author_id = u.id
LEFT JOIN guide_translation gt ON ag.id = gt.audio_guide_id

LEFT JOIN audio_guide_place agp ON ag.id = agp.guides_id
LEFT JOIN place p ON p.id = agp.places_id

LEFT JOIN languages l ON ag.original_language_id = l.id

WHERE ag.id = @audioGuideId;";

        var audioGuideDictionary = new Dictionary<Guid, AudioGuideResponse>();

        var result = await connection.QueryAsync<AudioGuideResponse, AudioGuideTranslationResponse, PlaceResponse, AudioGuideResponse>(
            sql,
            (audioGuide, translation, place) =>
            {
                if (!audioGuideDictionary.TryGetValue(audioGuide.Id, out var existingGuide))
                {
                    existingGuide = audioGuide;
                    audioGuideDictionary[audioGuide.Id] = existingGuide;
                }

                if (translation != null && !existingGuide.Translations.Any(t => t.TranslationLanguageId == translation.TranslationLanguageId))
                {
                    existingGuide.Translations.Add(translation);
                }

                if (place != null && !existingGuide.Places.Any(p => p.PlaceId == place.PlaceId))
                {
                    existingGuide.Places.Add(place);
                }

                return existingGuide;
            },
            new { audioGuideId },
            splitOn: "TranslationLanguageId,PlaceId"
        );

        var audioGuide = result.FirstOrDefault();
        return audioGuide ?? null;
    }

    // public async Task<PaginatedResponse<AudioGuideShortResponse>> GetAudioGuidesAsync(AudioGuideFilter filter)
    // {
    //     using var connection = _connectionFactory.GetDbConnection();
    //     var sqlBuilder = new StringBuilder(@"
    // SELECT 
    //     ag.id AS Id,
    //     ag.title AS Title, 
    //     ag.description AS Description, 
    //     ag.city AS City, 
    //     ag.price_amount AS PriceAmount, 
    //     ag.price_currency AS PriceCurrency, 
    //     ag.status AS Status, 
    //     ag.date_published AS DatePublished, 
    //     ag.date_edited AS DateEdited, 
    //     ag.author_id AS AuthorId, 
    //     ag.original_language_id AS OriginalLanguageId, 
    //     ag.audio_link AS AudioKey, 
    //     ag.image_link AS ImageKey
    // FROM audio_guides ag
    // WHERE 1=1 ");
    //
    //     var parameters = new DynamicParameters();
    //
    //     if (filter.CreatedByUserId is not null)
    //     {
    //         sqlBuilder.Append(" AND ag.author_id = @CreatedByUserId");
    //         parameters.Add("CreatedByUserId", filter.CreatedByUserId);
    //     }
    //
    //     if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
    //     {
    //         sqlBuilder.Append(@" AND (
    //         LOWER(ag.title) LIKE LOWER(@SearchPattern) OR
    //         LOWER(ag.description) LIKE LOWER(@SearchPattern) OR
    //         LOWER(ag.city) LIKE LOWER(@SearchPattern)
    //     )");
    //         parameters.Add("SearchPattern", $"%{filter.SearchQuery}%");
    //     }
    //
    //     if (filter.GetNewest)
    //     {
    //         sqlBuilder.Append(" ORDER BY ag.date_published DESC");
    //     }
    //     else
    //     {
    //         sqlBuilder.Append(" ORDER BY ag.title ASC");
    //     }
    //
    //     if (filter.GetTopN.HasValue)
    //     {
    //         sqlBuilder.Append(" LIMIT @Limit");
    //         parameters.Add("Limit", filter.GetTopN.Value);
    //     }
    //     else if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
    //     {
    //         sqlBuilder.Append(" LIMIT @Limit OFFSET @Offset");
    //         parameters.Add("Limit", filter.PageSize.Value);
    //         parameters.Add("Offset", (filter.PageNumber.Value - 1) * filter.PageSize.Value);
    //     }
    //
    //     var sql = sqlBuilder.ToString();
    //
    //     var countSql = new StringBuilder("SELECT COUNT(*) ");
    //     countSql.Append(sql);
    //
    //     var finalSql = $"{sql}; {countSql}";
    //
    //     using var multi = await connection.QueryMultipleAsync(finalSql, parameters);
    //     var data = (await multi.ReadAsync<AudioGuideShortResponse>()).ToList();
    //     var totalCount = await multi.ReadSingleAsync<int>();
    //
    //     return new PaginatedResponse<AudioGuideShortResponse>
    //     {
    //         Items = data,
    //         TotalCount = totalCount
    //     };
    // }

    public async Task<PaginatedResponse<AudioGuideShortResponse>> GetAudioGuidesAsync(AudioGuideFilter filter)
    {
        using var connection = _connectionFactory.GetDbConnection();

        var baseSql = new StringBuilder(@"
FROM audio_guides ag
WHERE 1=1 ");

        var parameters = new DynamicParameters();

        if (filter.CreatedByUserId is not null)
        {
            baseSql.Append(" AND ag.author_id = @CreatedByUserId");
            parameters.Add("CreatedByUserId", filter.CreatedByUserId);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
        {
            baseSql.Append(@" AND (
            LOWER(ag.title) LIKE LOWER(@SearchPattern) OR
            LOWER(ag.description) LIKE LOWER(@SearchPattern) OR
            LOWER(ag.city) LIKE LOWER(@SearchPattern)
        )");
            parameters.Add("SearchPattern", $"%{filter.SearchQuery}%");
        }

        // Main query with pagination
        var dataSql = new StringBuilder(@"
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
    ag.audio_link AS AudioKey, 
    ag.image_link AS ImageKey
");
        dataSql.Append(baseSql);

        if (filter.GetNewest)
        {
            dataSql.Append(" ORDER BY ag.date_published DESC");
        }
        else
        {
            dataSql.Append(" ORDER BY ag.title ASC");
        }

        if (filter.GetTopN.HasValue)
        {
            dataSql.Append(" LIMIT @Limit");
            parameters.Add("Limit", filter.GetTopN.Value);
        }
        else if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
        {
            dataSql.Append(" LIMIT @Limit OFFSET @Offset");
            parameters.Add("Limit", filter.PageSize.Value);
            parameters.Add("Offset", (filter.PageNumber.Value - 1) * filter.PageSize.Value);
        }

        // Count query
        var countSql = new StringBuilder("SELECT COUNT(*) ");
        countSql.Append(baseSql);

        var finalSql = $"{dataSql}; {countSql}";

        using var multi = await connection.QueryMultipleAsync(finalSql, parameters);

        var data = (await multi.ReadAsync<AudioGuideShortResponse>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PaginatedResponse<AudioGuideShortResponse>
        {
            Items = data,
            TotalCount = totalCount
        };
    }

    public async Task<IEnumerable<AudioGuideShortResponse>> GetLikedGuidesAsync(Guid userId, int page, int pageSize)
    {
        using var connection = _connectionFactory.GetDbConnection();

        var sql = @"
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
            ag.audio_link AS AudioKey,
            ag.image_link AS ImageKey
        FROM audio_guides ag
        INNER JOIN likes l ON l.entity_id = ag.id
        WHERE l.user_id = @UserId AND l.entity_type = @EntityType
        ORDER BY ag.date_published DESC
        LIMIT @PageSize OFFSET @Offset;
    ";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        parameters.Add("EntityType", "AudioGuide");
        parameters.Add("PageSize", pageSize);
        parameters.Add("Offset", (page - 1) * pageSize);

        return await connection.QueryAsync<AudioGuideShortResponse>(sql, parameters);
    }
}

