using EchoesOfUzbekistan.Application.Comments.GetCommentsForEntity;
using EchoesOfUzbekistan.Application.Comments.PostComment;
using EchoesOfUzbekistan.Application.Users.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers.Comments;
[Route("api/[controller]")]
[ApiController]
public class CommentsController : AppControllerBase
{
    public CommentsController(ISender sender, IUserContextService userContextService) : base(sender, userContextService)
    {
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] PostCommentCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCommentForEntity([FromQuery] GetCommentsForEntityQuery request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result.Value);
    }
}
