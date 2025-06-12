using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.GetUser;

namespace EchoesOfUzbekistan.Application.Users.GetUsers;

public record GetUsersQuery(UserFilter Filter) : IQuery<PaginatedResponse<UserShortResponse>>;
