using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Users.GetUser;
public record GetUserQuery(Guid userId) : IQuery<UserResponse>;
