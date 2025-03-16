using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.GetUser;

namespace EchoesOfUzbekistan.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;
