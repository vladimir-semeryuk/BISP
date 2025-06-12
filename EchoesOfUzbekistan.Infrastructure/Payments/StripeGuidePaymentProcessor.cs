using EchoesOfUzbekistan.Application.Abstractions.Payments;
using EchoesOfUzbekistan.Domain.Guides;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace EchoesOfUzbekistan.Infrastructure.Payments;
public class StripePaymentProcessor : IPaymentProcessor
{
    private readonly string _webhookSecret;
    public StripePaymentProcessor(IConfiguration config)
    {
        _webhookSecret = config["Stripe:WebhookSecret"];
    }
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
            Metadata = new Dictionary<string, string>
            {
                { "userId", userId.ToString() },
                { "guideId", guide.Id.ToString() }
            },
            SuccessUrl = $"http://localhost:4200/view-guide/{guide.Id}",
            CancelUrl = $"http://localhost:4200/view-guide/{guide.Id}"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }

    public (Guid userId, Guid guideId)? ExtractSessionData(string json, string stripeSignature)
    {
        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _webhookSecret);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }

        if (stripeEvent.Type != "checkout.session.completed")
            return null;

        var session = stripeEvent.Data.Object as Session;
        if (session?.Metadata == null) return null;

        if (Guid.TryParse(session.Metadata["userId"], out var userId) &&
            Guid.TryParse(session.Metadata["guideId"], out var guideId))
        {
            return (userId, guideId);
        }

        return null;
    }
}
