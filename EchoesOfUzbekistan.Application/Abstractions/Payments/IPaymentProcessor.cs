using EchoesOfUzbekistan.Domain.Guides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Abstractions.Payments;
public interface IPaymentProcessor
{
    Task<string> CreateCheckoutSessionAsync(AudioGuide guide, Guid userId);
}
