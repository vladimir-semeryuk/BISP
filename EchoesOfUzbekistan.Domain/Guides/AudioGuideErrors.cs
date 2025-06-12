using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Guides;
public static class AudioGuideErrors
{
    public static readonly Error NotFound = new(
        "AudioGuide.Found",
        "The audio guide with the specified id was not found");

    public static readonly Error FreeGuide = new(
        "AudioGuide.FreeGuide",
        "You cannot pay for a free guide.");

    public static readonly Error AlreadyPurchased = new(
        "AudioGuide.AlreadyPurchased",
        "You have already purchased this guide.");

    public static readonly Error PurchaseError = new(
        "AudioGuide.PurchaseError",
        "There was an error with processing the payment.");
}
