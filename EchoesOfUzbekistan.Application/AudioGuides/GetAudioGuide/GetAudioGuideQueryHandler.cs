﻿using Dapper;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Places.GetPlace;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
internal class GetAudioGuideQueryHandler : IQueryHandler<GetAudioGuideQuery, AudioGuideResponse>
{
    private readonly ISQLConnectionFactory _connectionFactory;
    public GetAudioGuideQueryHandler(ISQLConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Result<AudioGuideResponse>> Handle(GetAudioGuideQuery request, CancellationToken cancellationToken)
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
        ag.original_language_id AS OriginalLanguageId, 
        ag.audio_link AS AudioLink, 
        ag.image_link AS ImageLink,
        -- Translations
        gt.language_id AS TranslationLanguageId,
        gt.title AS Title,
        gt.description AS Description,
        -- Places
        p.id AS PlaceId,
        p.title AS Title,
        p.description AS Description,
        ST_AsGeoJSON(p.coordinates) AS Coordinates,
        p.status AS Status,
        p.date_published AS DatePublished,
        p.date_edited AS DateEdited,
        p.author_id AS AuthorId,
        p.original_language_id AS OriginalLanguageId,
        p.audio_link AS AudioLink, 
        p.image_link AS ImageLink
        FROM audio_guides ag
        LEFT JOIN guide_translation gt ON ag.id = gt.audio_guide_id
        LEFT JOIN place p ON ag.id = p.audio_guide_id
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
            new { request.AudioGuideId },
            splitOn: "TranslationLanguageId,PlaceId"
        );

        var audioGuide = result.FirstOrDefault();
        return audioGuide ?? null;
    }
}
