namespace EchoesOfUzbekistan.Application.Comments.GetCommentsForEntity;

public class CommentForEntityDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
    public string AuthorAvatar { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; }
    public DateTime DateCreated { get; set; }
}