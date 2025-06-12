using EchoesOfUzbekistan.Domain.Guides;

namespace EchoesOfUzbekistan.Application.Abstractions.Payments;
public interface IPaymentProcessor
{
    Task<string> CreateCheckoutSessionAsync(AudioGuide guide, Guid userId);
    (Guid userId, Guid guideId)? ExtractSessionData(string json, string stripeSignature);
}
