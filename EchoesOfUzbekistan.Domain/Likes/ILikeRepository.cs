namespace EchoesOfUzbekistan.Domain.Likes;
public interface ILikeRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid entityId, string entityType);
    Task<Like?> GetAsync(Guid userId, Guid entityId, string entityType);
    void Add(Like like);
    void Remove(Like like);
}
