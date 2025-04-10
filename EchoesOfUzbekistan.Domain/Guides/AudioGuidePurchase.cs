using EchoesOfUzbekistan.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Domain.Guides;
public class AudioGuidePurchase : Entity
{
    public Guid UserId { get; private set; }
    public Guid GuideId { get; private set; }
    public DateTime PurchaseDate { get; private set; }

    public AudioGuidePurchase(Guid userId, Guid guideId)
    {
        UserId = userId;
        GuideId = guideId;
        PurchaseDate = DateTime.UtcNow;
    }
}

