using Dapper;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Places.GetPlaces;
using EchoesOfUzbekistan.Application.Places.Interfaces;
using System.Text;
using EchoesOfUzbekistan.Application.Places.GetPlace;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Places;
public class PlaceReadRepository : IPlaceReadRepository
{
    private readonly ISQLConnectionFactory _connectionFactory;

    public PlaceReadRepository(ISQLConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PaginatedResponse<PlaceResponse>> GetPlacesAsync(PlaceFilter filter)
    {
        using var connection = _connectionFactory.GetDbConnection();

        var sqlBuilder = new StringBuilder(@"
SELECT 
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
    p.image_link AS ImageKey,
    (
        SELECT array_agg(agp.guides_id)
        FROM audio_guide_place agp
        WHERE agp.places_id = p.id
    ) AS AudioGuideIds
FROM place p
WHERE 1=1");

        var parameters = new DynamicParameters();

        if (filter.CreatedByUserId is not null)
        {
            sqlBuilder.Append(" AND p.author_id = @CreatedByUserId");
            parameters.Add("CreatedByUserId", filter.CreatedByUserId);
        }

        if (filter.GetNewest)
        {
            sqlBuilder.Append(" ORDER BY p.date_published DESC");
        }
        else
        {
            sqlBuilder.Append(" ORDER BY p.title ASC");
        }

        if (filter.GetTopN.HasValue)
        {
            sqlBuilder.Append(" LIMIT @Limit");
            parameters.Add("Limit", filter.GetTopN.Value);
        }
        else if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
        {
            sqlBuilder.Append(" LIMIT @Limit OFFSET @Offset");
            parameters.Add("Limit", filter.PageSize.Value);
            parameters.Add("Offset", (filter.PageNumber.Value - 1) * filter.PageSize.Value);
        }

        // Second: count query
        sqlBuilder.Append(@";
SELECT COUNT(*) FROM place p WHERE 1=1
");

        if (filter.CreatedByUserId is not null)
        {
            sqlBuilder.Append(" AND p.author_id = @CreatedByUserId");
            // Already added
        }

        var sql = sqlBuilder.ToString();

        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var data = (await multi.ReadAsync<PlaceResponse>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PaginatedResponse<PlaceResponse>
        {
            Items = data,
            TotalCount = totalCount
        };
    }

    public async Task<PlaceDetailsResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.GetDbConnection();

        const string sql = @"
SELECT 
    p.id AS PlaceId,
    p.title AS Title,
    p.description AS Description,
    ST_AsGeoJSON(p.coordinates) AS Coordinates,
    p.status AS Status,
    p.date_published AS DatePublished,
    p.date_edited AS DateEdited,
    p.author_id AS AuthorId,
    l.code AS LanguageCode,
    p.original_language_id AS OriginalLanguageId,
    p.audio_link AS AudioKey,
    p.image_link AS ImageKey,
    (
        SELECT array_agg(agp.guides_id)
        FROM audio_guide_place agp
        WHERE agp.places_id = p.id
    ) AS AudioGuideIds,
    ag.id AS AudioGuideId,
    ag.title AS AudioGuideTitle
FROM place p
LEFT JOIN languages l ON l.id = p.original_language_id
LEFT JOIN audio_guide_place agp ON agp.places_id = p.id
LEFT JOIN audio_guides ag ON ag.id = agp.guides_id
WHERE p.Id = @Id";

        var placeMap = new Dictionary<Guid, PlaceDetailsResponse>();

        var result = await connection.QueryAsync<PlaceDetailsResponse, Guid?, string?, PlaceDetailsResponse>(
            sql,
            (place, guideId, guideTitle) =>
            {
                if (!placeMap.TryGetValue(place.PlaceId, out var existingPlace))
                {
                    existingPlace = place;
                    existingPlace.AudioGuidesIds = new List<Guid>();
                    existingPlace.AudioGuidesNames = new List<string>();
                    placeMap.Add(place.PlaceId, existingPlace);
                }

                if (guideId.HasValue)
                {
                    existingPlace.AudioGuidesIds.Add(guideId.Value);
                    if (!string.IsNullOrWhiteSpace(guideTitle))
                        existingPlace.AudioGuidesNames.Add(guideTitle);
                }

                return existingPlace;
            },
            new { Id = id },
            splitOn: "AudioGuideId,AudioGuideTitle"
        );

        return result.FirstOrDefault();
    }
}
