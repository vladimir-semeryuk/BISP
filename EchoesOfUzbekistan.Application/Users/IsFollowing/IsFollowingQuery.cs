using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Users.IsFollowing;
public record IsFollowingQuery(Guid UserId) : IQuery<bool>;