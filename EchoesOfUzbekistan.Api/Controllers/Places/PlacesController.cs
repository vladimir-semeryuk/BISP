using EchoesOfUzbekistan.Application.Places.GetPlace;
using EchoesOfUzbekistan.Application.Places.PostPlace;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers.Places;
[Route("api/[controller]")]
[ApiController]
public class PlacesController : AppControllerBase
{
    public PlacesController(ISender sender, IUserContextService userContextService) : base(sender, userContextService)
    {
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlace(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPlaceQuery(id);

        Result<PlaceResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlace(
        CreatePlaceRequest req,
        CancellationToken cancellationToken)
    {
        var command = new PostPlaceCommand(
            req.Title,
            req.Description,
            req.Longitude,
            req.Latitude,
            req.LanguageCode,
            req.AuthorId,
            req.AudioLink,
            req.ImageLink);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }


        return Ok(result.Value);
    }
}
