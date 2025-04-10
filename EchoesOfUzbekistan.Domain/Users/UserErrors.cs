using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static readonly Error AlreadyFollow = new(
        "User.AlreadyFollow",
        "You already follow the user.");

    public static readonly Error CannotPostForOthers = new(
        "User.CannotPostForOthers",
        "You already cannot post for other users.");

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The provided credentials were invalid");
}
