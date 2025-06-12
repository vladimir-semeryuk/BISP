using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Users.GetFollowers;

public record GetFollowersQuery(Guid Id, int PageNumber = 1, int PageSize = 20) : IQuery<PaginatedResponse<FriendResponse>>;
