using EchoesOfUzbekistan.Application.Abstractions.Payments;
using EchoesOfUzbekistan.Domain.Guides;
using Stripe.Checkout;

namespace EchoesOfUzbekistan.Infrastructure.Payments;
public class StripePaymentProcessor : IPaymentProcessor
{
    public async Task<string> CreateCheckoutSessionAsync(AudioGuide guide, Guid userId)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = guide.Price.Currency.Code.ToLowerInvariant(),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = guide.Title.guideTitle
                        },
                        UnitAmount = (long)(guide.Price.Amount * 100)
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = "http://localhost:4200/",
            CancelUrl = "http://localhost:4200/login"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }
}
