using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Domain.Comments;
public class Comment : Entity
{
    public Guid UserId { get; private set; }
    public Guid EntityId { get; private set; }
    public string EntityType { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Comment(Guid userId, Guid entityId, string entityType, string content)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        EntityId = entityId;
        EntityType = entityType;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }
}

