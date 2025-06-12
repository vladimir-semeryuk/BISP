using EchoesOfUzbekistan.Application.Likes.GetLikedEntities;

namespace EchoesOfUzbekistan.Application.Likes.Interfaces;
public interface ILikeReadRepository
{
    Task<int> GetLikeCountAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default);
    Task<bool> HasUserLikedAsync(Guid userId, Guid entityId, string entityType, CancellationToken cancellationToken = default);
    Task<IEnumerable<LikedAudioGuideDto>> GetLikedAudioGuidesAsync(Guid userId, int pageNumber, int pageSize);
}

