using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Users;
public static class UserErrors
{
    public static readonly Error NotFound = new(
        "User.Found",
        "The user with the specified id was not found");

    public static readonly Error AuthorNotFound = new(
        "User.AuthorFound",
        "The author with the specified id was not found");

    public static readonly Error CannotFollowYourself = new(
        "User.CannotFollowYourself",
        "You cannot follow yourself.");

    public static readonly Error CannotUnfollowYourself = new(
        "User.CannotUnfollowYourself",
        "You cannot unfollow yourself.");

    public static readonly Error AlreadyFollow = new(
        "User.AlreadyFollow",
        "You already follow the user.");

    public static readonly Error FollowNotFound = new(
        "User.FollowNotFound",
        "You aren't following this user.");

    public static readonly Error CannotPostForOthers = new(
        "User.CannotPostForOthers",
        "You cannot post for other users.");

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The provided credentials were invalid");

    public static readonly Error InvalidRefreshToken = new(
        "User.InvalidRefreshToken",
        "The provided refresh token is invalid");
    public static readonly Error UpdateImageWithNullImageLink = new(
        "User.UpdateImageWithNullImageLink",
        "The profile picture cannot be updated without a valid URL.");
}
