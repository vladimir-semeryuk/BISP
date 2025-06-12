using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Users.GetUser;
using EchoesOfUzbekistan.Application.Users.Interfaces;
using System.Data;
using Dapper;
using EchoesOfUzbekistan.Application.Users.GetFollowers;
using EchoesOfUzbekistan.Application.Users.GetUsers;

namespace EchoesOfUzbekistan.Infrastructure.Repositories.Users;
public class UserReadRepository : IUserReadRepository
{
    private readonly ISQLConnectionFactory _sqlConnectionFactory;

    public UserReadRepository(ISQLConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.GetDbConnection();
        var sql = @"SELECT u.id AS Id,
                    u.first_name AS FirstName,
                    u.surname AS Surname,
                    u.email AS Email,
                    u.registration_date_utc AS RegistrationDateUtc, 
                    u.country_name AS CountryName,
                    u.country_iso_code AS CountryCode,
                    u.city AS City,
                    u.about_me AS AboutMe,
                    u.image_link AS ImageKey,
                    (SELECT COUNT(*)
                     FROM friendships f1
                     INNER JOIN friendships f2 
                         ON f1.follower_id = f2.followee_id 
                        AND f1.followee_id = f2.follower_id
                     WHERE f1.follower_id < f1.followee_id  -- prevent double-counting
                       AND (f1.follower_id = u.id OR f1.followee_id = u.id)) AS FriendCount
                    FROM users u
                    WHERE id = @userId;";
        var user = await connection.QueryFirstOrDefaultAsync<UserResponse>(
            sql,
            new
            {
                userId = id
            });
        return user;
    }

    public async Task<UserResponse?> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.GetDbConnection();

        const string sql = @"
    SELECT 
        u.id AS Id,
        u.first_name AS FirstName,
        u.surname AS Surname,
        u.email AS Email,
        u.registration_date_utc AS RegistrationDateUtc, 
        u.country_name AS CountryName,
        u.country_iso_code AS CountryCode,
        u.city AS City,
        u.about_me AS AboutMe,
        u.image_link AS ImageKey,
        (SELECT COUNT(*)
         FROM friendships f1
         INNER JOIN friendships f2 
             ON f1.follower_id = f2.followee_id 
            AND f1.followee_id = f2.follower_id
         WHERE f1.follower_id < f1.followee_id
           AND (f1.follower_id = u.id OR f1.followee_id = u.id)) AS FriendCount,
        r.name AS RoleName
    FROM users u
    LEFT JOIN role_user ur ON u.id = ur.users_id
    LEFT JOIN roles r ON r.id = ur.roles_id
    WHERE u.identity_id = @IdentityId;";

        var userDictionary = new Dictionary<Guid, UserResponse>();

        var result = await connection.QueryAsync<UserResponse, string, UserResponse>(
            sql,
            (user, roleName) =>
            {
                if (!userDictionary.TryGetValue(user.Id, out var existingUser))
                {
                    existingUser = new UserResponse
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        Surname = user.Surname,
                        Email = user.Email,
                        RegistrationDateUtc = user.RegistrationDateUtc,
                        CountryName = user.CountryName,
                        CountryCode = user.CountryCode,
                        City = user.City,
                        AboutMe = user.AboutMe,
                        ImageLink = user.ImageLink,
                        ImageKey = user.ImageKey,
                        FriendCount = user.FriendCount,
                        Roles = Array.Empty<string>()  // Initialize empty array
                    };
                    userDictionary.Add(existingUser.Id, existingUser);
                }

                if (!string.IsNullOrEmpty(roleName))
                {
                    existingUser.Roles = existingUser.Roles.Append(roleName).ToArray();
                }

                return existingUser;
            },
            new { IdentityId = identityId },
            splitOn: "RoleName"
        );

        return userDictionary.Values.SingleOrDefault();
    }

    public async Task<PaginatedResponse<FriendResponse>> GetMutualFollowersAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.GetDbConnection();

        var countSql = @"
        SELECT COUNT(*)
        FROM friendships f1
        INNER JOIN friendships f2 ON f1.followee_id = f2.follower_id AND f1.follower_id = f2.followee_id
        WHERE f1.follower_id = @UserId";

        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new { UserId = userId });

        var sql = @"
        SELECT 
        u.id AS FriendId,
        u.first_name AS FirstName,
        u.surname AS Surname,
        u.image_link AS ImageKey
        FROM friendships f1
        INNER JOIN friendships f2 ON f1.followee_id = f2.follower_id AND f1.follower_id = f2.followee_id
        INNER JOIN users u ON u.id = f1.followee_id
        WHERE f1.follower_id = @UserId
        LIMIT @Limit OFFSET @Offset";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        parameters.Add("Limit", pageSize);
        parameters.Add("Offset", (pageNumber - 1) * pageSize);

        var items = (await connection.QueryAsync<FriendResponse>(sql, parameters)).ToList();

        return new PaginatedResponse<FriendResponse>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponse<FriendResponse>> GetFollowersAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.GetDbConnection();

        var countSql = "SELECT COUNT(*) FROM friendships WHERE followee_id = @UserId";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new { UserId = userId });

        var sql = @"
        SELECT 
        u.id AS FriendId,
        u.first_name AS FirstName,
        u.surname AS Surname,
        u.image_link AS ImageKey
        FROM friendships f
        INNER JOIN users u ON f.follower_id = u.id
        WHERE f.followee_id = @UserId
        LIMIT @Limit OFFSET @Offset";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        parameters.Add("Limit", pageSize);
        parameters.Add("Offset", (pageNumber - 1) * pageSize);

        var items = (await connection.QueryAsync<FriendResponse>(sql, parameters)).ToList();

        return new PaginatedResponse<FriendResponse>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<PaginatedResponse<FriendResponse>> GetFolloweesAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = _sqlConnectionFactory.GetDbConnection();

        var countSql = "SELECT COUNT(*) FROM friendships WHERE follower_id = @UserId";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new { UserId = userId });

        var sql = @"
        SELECT 
        u.id AS FriendId,
        u.first_name AS FirstName,
        u.surname AS Surname,
        u.image_link AS ImageKey
        FROM friendships f
        INNER JOIN users u ON f.followee_id = u.id
        WHERE f.follower_id = @UserId
        LIMIT @Limit OFFSET @Offset";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        parameters.Add("Limit", pageSize);
        parameters.Add("Offset", (pageNumber - 1) * pageSize);

        var items = (await connection.QueryAsync<FriendResponse>(sql, parameters)).ToList();

        return new PaginatedResponse<FriendResponse>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<bool> IsFollowingAsync(Guid currentUserId, Guid targetUserId)
    {
        using IDbConnection connection = _sqlConnectionFactory.GetDbConnection();

        var sql = @"
        SELECT COUNT(1)
        FROM friendships
        WHERE follower_id = @CurrentUserId
          AND followee_id = @TargetUserId
    ";

        var result = await connection.ExecuteScalarAsync<int>(sql, new
        {
            CurrentUserId = currentUserId,
            TargetUserId = targetUserId
        });

        return result > 0;
    }

    public async Task<PaginatedResponse<UserShortResponse>> GetUsersAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.GetDbConnection();

        var countSql = @"
        SELECT COUNT(*)
        FROM users u
        WHERE (@SearchTerm IS NULL OR LOWER(u.first_name) LIKE LOWER(@SearchTerm) OR u.surname LIKE @SearchTerm OR u.email LIKE @SearchTerm);
    ";

        var searchTerm = string.IsNullOrEmpty(filter.SearchTerm) ? null : $"%{filter.SearchTerm}%";

        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new { SearchTerm = searchTerm });

        var sql = @"
        SELECT u.id AS Id,
               u.first_name AS FirstName,
               u.surname AS Surname,
               u.email AS Email,
               u.about_me AS AboutMe,
               u.image_link AS ImageKey
        FROM users u
        WHERE (@SearchTerm IS NULL OR LOWER(u.first_name) LIKE LOWER(@SearchTerm) OR u.surname LIKE @SearchTerm OR u.email LIKE @SearchTerm)
        ORDER BY u.registration_date_utc DESC
        LIMIT @Limit OFFSET @Offset;
    ";

        var parameters = new
        {
            SearchTerm = searchTerm,
            Limit = filter.PageSize,
            Offset = (filter.Page - 1) * filter.PageSize
        };

        var users = (await connection.QueryAsync<UserShortResponse>(sql, parameters)).ToList();

        return new PaginatedResponse<UserShortResponse>
        {
            Items = users,
            TotalCount = totalCount
        };
    }
}
