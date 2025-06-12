using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Likes.HasLiked;

public record HasLikedQuery(Guid UserId, Guid EntityId, string EntityType) : IQuery<bool>;