using EchoesOfUzbekistan.Application.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EchoesOfUzbekistan.Application.Users.SignupUser;
public record SignupUserCommand(
    string Email,
    string FirstName,
    string Surname,
    string Password,
    string CountryName,
    string CountryCode) : ICommand<Guid>;
