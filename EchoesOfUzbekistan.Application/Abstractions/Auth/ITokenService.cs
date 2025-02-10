using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Abstractions.Auth;
public interface ITokenService
{
    Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default);
}
