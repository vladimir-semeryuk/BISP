using EchoesOfUzbekistan.Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Abstractions.Messages;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
