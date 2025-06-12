using EchoesOfUzbekistan.Application.Likes.GetLikedEntities;
using EchoesOfUzbekistan.Application.Likes.HasLiked;
using EchoesOfUzbekistan.Application.Likes.LikeEntity;
using EchoesOfUzbekistan.Application.Likes.UnlikeEntity;
using EchoesOfUzbekistan.Application.Users.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers.Likes;
[Route("api/[controller]")]
[ApiController]
public class LikesController : AppControllerBase
{
    public LikesController(ISender sender, IUserContextService userContextService) : base(sender, userContextService)
    {
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Like([FromBody] LikeRequest request)
    {
        var result = await _sender.Send(new LikeEntityCommand(request.EntityId, request.EntityType));
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("unlike")]
    public async Task<IActionResult> Unlike([FromBody] LikeRequest request)
    {
        var result = await _sender.Send(new UnlikeEntityCommand(request.EntityId, request.EntityType));
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("has-liked")]
    public async Task<IActionResult> HasLiked([FromQuery] Guid entityId, [FromQuery] string entityType)
    {
        var userId = CurrentUserId;
        var result = await _sender.Send(new HasLikedQuery(userId, entityId, entityType));
        if (result.IsFailure)
        {
            return BadRequest();
        }
        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet("liked")]
    public async Task<IActionResult> GetLikedAudioGuides([FromQuery] GetLikedRequest request)
    {
        Guid? userId = CurrentUserId;
        if (request.UserId.HasValue)
        {
            userId = request.UserId;
        }

        if (userId == Guid.Empty)
        {
            return BadRequest("User ID is required.");
        }

        var result = await _sender.Send(new GetLikedEntitiesQuery(userId.Value, request.PageNumber, request.PageSize));

        return Ok(result.Value);
    }
}