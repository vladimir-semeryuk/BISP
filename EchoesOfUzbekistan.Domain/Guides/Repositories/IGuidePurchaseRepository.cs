using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Guides.Repositories;
public interface IGuidePurchaseRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid guideId);
    Task AddAsync(AudioGuidePurchase purchase);
    Task<List<Guid>> GetPurchasedGuideIdsAsync(Guid userId);
}
