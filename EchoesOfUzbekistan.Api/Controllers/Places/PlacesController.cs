using EchoesOfUzbekistan.Application.Places.DeletePlace;
using EchoesOfUzbekistan.Application.Places.EditPlace;
using EchoesOfUzbekistan.Application.Places.GetPlace;
using EchoesOfUzbekistan.Application.Places.GetPlaces;
using EchoesOfUzbekistan.Application.Places.PostPlace;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Application.Users.UpdateUser;
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

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPlaces([FromQuery] PlaceFilter filter, CancellationToken cancellationToken)
    {
        var query = new GetPlacesQuery(filter);

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
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
            req.ImageLink,
            req.AudioGuidesIds);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }


        return Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{placeId}")]
    public async Task<IActionResult> DeletePlace(Guid placeId, CancellationToken cancellationToken)
    {
        var command = new DeletePlaceCommand(placeId);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.Error);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlace([FromBody] UpdatePlaceRequest request, CancellationToken cancellationToken)
    {
        var command = new EditPlaceCommand(
            request.PlaceId,
            request.Title,
            request.Description,
            request.Latitude,
            request.Longitude,
            request.LanguageCode,
            request.AudioLink,
            request.ImageLink,
            request.AudioGuidesIds);

        var result = await _sender.Send(command, cancellationToken);
        if (result.Error == Error.UnauthorizedAccess)
            return Unauthorized(result);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
