using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Likes.GetLikedEntities;

public record GetLikedEntitiesQuery(Guid UserId, int PageNumber, int PageSize) : IQuery<IEnumerable<LikedAudioGuideDto>>;