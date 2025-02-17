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

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The provided credentials were invalid");
}
