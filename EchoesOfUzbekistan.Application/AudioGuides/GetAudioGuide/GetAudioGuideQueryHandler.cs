using Dapper;
using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    id AS Id,
                    title AS Title, 
                    description AS Description, 
                    city AS City, 
                    price_amount AS PriceAmount, 
                    price_currency AS PriceCurrency, 
                    status AS Status, 
                    date_published AS DatePublished, 
                    date_edited AS DateEdited, 
                    author_id AS AuthorId, 
                    original_language_id AS OriginalLanguageId, 
                    audio_link AS AudioLink, 
                    image_link AS ImageLink
                    FROM audio_guides
                    WHERE id = @audioGuideId";
        var audioGuide = await connection.QueryFirstOrDefaultAsync<AudioGuideResponse>(
            sql,
            new
            {
                request.audioGuideId
            });
        return audioGuide;
    }
}
