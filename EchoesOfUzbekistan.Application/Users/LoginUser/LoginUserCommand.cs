using EchoesOfUzbekistan.Application.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Users.LoginUser;
public record LoginUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;
