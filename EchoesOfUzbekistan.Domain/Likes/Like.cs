using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Likes;
public class Like : Entity
{
    public Guid UserId { get; private set; }
    public Guid EntityId { get; private set; }
    public string EntityType { get; private set; }

    private Like() {} 
    public Like(Guid userId, Guid entityId, string entityType)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        EntityId = entityId;
        EntityType = entityType;
    }
}
