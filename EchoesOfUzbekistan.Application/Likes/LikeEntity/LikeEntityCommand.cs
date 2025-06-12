using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Likes.LikeEntity;

public record LikeEntityCommand(Guid EntityId, string EntityType) : ICommand;
