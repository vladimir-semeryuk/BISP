using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Comments.PostComment;

public record PostCommentCommand(Guid EntityId, string EntityType, string Content) : ICommand<Guid>;

