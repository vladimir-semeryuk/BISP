using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.Payments;
using EchoesOfUzbekistan.Application.AudioGuides.ConfirmPurchase;
using EchoesOfUzbekistan.Application.AudioGuides.DeleteAudioGuide;
using EchoesOfUzbekistan.Application.AudioGuides.EditAudioGuide;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;
using EchoesOfUzbekistan.Application.AudioGuides.GetGuidePurchaseStatusForUser;
using EchoesOfUzbekistan.Application.AudioGuides.PostAudioGuide;
using EchoesOfUzbekistan.Application.AudioGuides.PurchaseAudioGuide;
using EchoesOfUzbekistan.Application.Places.DeletePlace;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;

namespace EchoesOfUzbekistan.Api.Controllers.AudioGuides;
[Route("api/[controller]")]
[ApiController]
public class AudioGuidesController : AppControllerBase
{
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IFileService _fileService;

    public AudioGuidesController(
        ISender sender, 
        IUserContextService userContextService, 
        IPaymentProcessor paymentProcessor,
        IFileService fileService) : base(sender, userContextService)
    {
        _paymentProcessor = paymentProcessor;
        _fileService = fileService;
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

        Result<PaginatedResponse<AudioGuideShortResponse>> result = await _sender.Send(query, cancellationToken);

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
            req.LanguageCode,
            req.GuideStatus,
            req.AudioLink,
            req.ImageLink,
            req.PlacesIds);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }


        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{guideId}")]
    public async Task<IActionResult> DeleteGuide(Guid guideId, CancellationToken cancellationToken)
    {
        var command = new DeleteAudioGuideCommand(guideId);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : BadRequest(result.Error);
    }

    [Authorize]
    [HttpPost("checkout/{guideId}")]
    public async Task<IActionResult> CreateCheckoutSession(Guid guideId)
    {
        var result = await _sender.Send(new PurchaseAudioGuideCommand(guideId));

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("stripe/webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];

        var sessionData = _paymentProcessor.ExtractSessionData(json, signature);

        if (sessionData is null)
            return BadRequest("Invalid webhook or unsupported event.");

        var (userId, guideId) = sessionData.Value;
        var result = await _sender.Send(new ConfirmPurchaseCommand(userId, guideId));

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }

    [Authorize]
    [HttpGet("payment-status/{guideId}")]
    public async Task<IActionResult> GetGuidePurchaseStatus(Guid userId, Guid guideId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetGuidePurchaseStatusForUserQuery(userId, guideId));
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet("stream/{guideId}")]
    public async Task<IActionResult> StreamAudioGuide(Guid guideId, CancellationToken cancellationToken)
    {
        // Get the guide to check ownership/purchase status
        var guideQuery = new GetAudioGuideQuery(guideId);
        var guideResult = await _sender.Send(guideQuery, cancellationToken);

        if (guideResult.IsFailure)
            return NotFound();

        var guide = guideResult.Value;

        // Check if user has purchased the guide
        var purchaseStatusQuery = new GetGuidePurchaseStatusForUserQuery(_userContextService.UserId, guideId);
        var purchaseStatus = await _sender.Send(purchaseStatusQuery, cancellationToken);

        if (purchaseStatus.IsFailure || !purchaseStatus.Value)
            return Unauthorized("You must purchase this guide to listen to it");

        if (string.IsNullOrEmpty(guide.AudioKey))
            return NotFound("Audio file not found");

        // Get the stream from the file service
        var stream = await _fileService.GetStreamAsync(guide.AudioKey, cancellationToken);
        
        if (stream == null)
            return NotFound("Audio file not found");

        // Return the stream with appropriate headers
        return File(stream, "audio/mpeg", enableRangeProcessing: true);
    }
}
