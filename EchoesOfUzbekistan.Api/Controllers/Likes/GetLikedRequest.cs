namespace EchoesOfUzbekistan.Api.Controllers.Likes;

public record GetLikedRequest(Guid? UserId, int PageNumber, int PageSize);