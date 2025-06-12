using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.GetFollowers;

namespace EchoesOfUzbekistan.Application.Users.GetMutualFollows;
public record GetMutualFollowsQuery(Guid Id, int PageNumber = 1, int PageSize = 20) : IQuery<PaginatedResponse<FriendResponse>>;