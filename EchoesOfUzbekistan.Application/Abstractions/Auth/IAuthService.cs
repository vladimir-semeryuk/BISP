using EchoesOfUzbekistan.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Abstractions.Auth;
public interface IAuthService
{
    Task<string> Signup(
        User user, 
        string password,
        CancellationToken cancellationToken = default);
}
