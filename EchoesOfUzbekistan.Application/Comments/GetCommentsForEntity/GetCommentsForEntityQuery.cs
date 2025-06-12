using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Comments.GetCommentsForEntity;

public record GetCommentsForEntityQuery(Guid EntityId, string EntityType) : IQuery<List<CommentForEntityDto>>;