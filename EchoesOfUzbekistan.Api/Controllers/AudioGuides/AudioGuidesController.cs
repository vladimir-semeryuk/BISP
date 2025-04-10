using EchoesOfUzbekistan.Application.AudioGuides.EditAudioGuide;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;
using EchoesOfUzbekistan.Application.AudioGuides.PostAudioGuide;
using EchoesOfUzbekistan.Application.AudioGuides.PurchaseAudioGuide;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers.AudioGuides;
[Route("api/[controller]")]
[ApiController]
public class AudioGuidesController : AppControllerBase
{
    public AudioGuidesController(ISender sender, IUserContextService userContextService) : base(sender, userContextService)
    {
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAudioGuide(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAudioGuideQuery(id);

        Result<AudioGuideResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> GetAudioGuides([FromQuery] AudioGuideFilter filter, CancellationToken cancellationToken)
    {
        var query = new GetAudioGuidesQuery(filter);

        Result<IReadOnlyList<AudioGuideShortResponse>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAudioGuide(
        CreateAudioGuideRequest req,
        CancellationToken cancellationToken)
    {
        var command = new PostAudioGuideCommand(
            req.Title,
            req.Description,
            req.City,
            req.MoneyAmount,
            req.CurrencyCode,
            req.LanguageCode,
            req.AuthorId,
            req.AudioLink,
            req.ImageLink,
            req.PlaceIds);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }


        return Ok(result.Value);
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAudioGuide(
        UpdateAudioGuideRequest req,
        CancellationToken cancellationToken)
    {
        var command = new EditAudioGuideCommand(
            req.GuideId,
            req.Title,
            req.Description,
            req.City,
            req.MoneyAmount,
            req.CurrencyCode,
            req.LanguageId,
            req.GuideStatus,
            req.AudioLink,
            req.ImageLink);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }


        return Ok(result);
    }

    [Authorize]
    [HttpPost("checkout/{guideId}")]
    public async Task<IActionResult> CreateCheckoutSession(Guid guideId)
    {
        var result = await _sender.Send(new PurchaseAudioGuideCommand(guideId));

        return Ok(result);
    }
}
