using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Abstractions.Auth;
public static class AuthorizationGuard
{
    public static Result EnsureUserOwnsResource(Guid currentUserId, Guid resourceOwnerId)
    {
        return currentUserId == resourceOwnerId
            ? Result.Success()
            : Result.Failure(Error.UnauthorizedAccess);
    }

    public static Result EnsureUserIsAdminModerator(IUserContextService context)
    {
        // If not the owner, check if the user is an admin/moderator
        var userRoles = context.Roles;
        bool isAdminOrModerator = userRoles?.Any(role => role == Role.Administrator.Name || role == Role.Moderator.Name) ?? false;

        return isAdminOrModerator
            ? Result.Success()
            : Result.Failure(Error.UnauthorizedAccess);
    }

    public static Result EnsureUserIsAdmin(IUserContextService context)
    {
        var userRoles = context.Roles;
        bool isAdmin = userRoles?.Any(role => role == Role.Administrator.Name) ?? false;

        return isAdmin
            ? Result.Success()
            : Result.Failure(Error.UnauthorizedAccess);
    }
}
