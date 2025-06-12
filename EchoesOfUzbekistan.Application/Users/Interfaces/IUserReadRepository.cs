using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Users.GetFollowers;
using EchoesOfUzbekistan.Application.Users.GetUser;
using EchoesOfUzbekistan.Application.Users.GetUsers;

namespace EchoesOfUzbekistan.Application.Users.Interfaces;
public interface IUserReadRepository
{
    Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserResponse?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<FriendResponse>> GetMutualFollowersAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<FriendResponse>> GetFollowersAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<FriendResponse>> GetFolloweesAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> IsFollowingAsync(Guid userId, Guid targetId);
    Task<PaginatedResponse<UserShortResponse>> GetUsersAsync(UserFilter  filter, CancellationToken cancellationToken = default);
}
