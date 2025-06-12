using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Likes.UnlikeEntity;
public record UnlikeEntityCommand(Guid EntityId, string EntityType) : ICommand;
