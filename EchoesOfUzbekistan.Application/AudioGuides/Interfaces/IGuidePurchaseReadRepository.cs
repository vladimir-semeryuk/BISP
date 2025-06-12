namespace EchoesOfUzbekistan.Application.AudioGuides.Interfaces;

public interface IGuidePurchaseReadRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid guideId);
    Task<List<Guid>> GetPurchasedGuideIdsAsync(Guid userId);
}
