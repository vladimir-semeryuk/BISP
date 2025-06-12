using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Guides;
public class AudioGuidePurchase : Entity
{
    public Guid UserId { get; private set; }
    public Guid GuideId { get; private set; }
    public DateTime PurchaseDate { get; private set; }

    public AudioGuidePurchase(Guid userId, Guid guideId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        GuideId = guideId;
        PurchaseDate = DateTime.UtcNow;
    }
}

