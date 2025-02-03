using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Users.GetUser;
public class UserResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
    public DateTime RegistrationDateUtc { get; init; }
    public string CountryName { get; init; }
    public string CountryCode { get; init; }
    public string? City { get; init; }
    public string? AboutMe { get; init; }
}
