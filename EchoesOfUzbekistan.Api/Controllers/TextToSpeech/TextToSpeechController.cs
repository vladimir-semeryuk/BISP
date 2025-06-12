using EchoesOfUzbekistan.Application.TextToSpeech.SynthesizeAudio;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers.TextToSpeech;
[Route("api/[controller]")]
[ApiController]
public class TextToSpeechController : AppControllerBase
{
    public TextToSpeechController(ISender sender, IUserContextService userContextService) : base(sender, userContextService)
    {
    }

    [Authorize]
    [HttpPost("synthesize")]
    public async Task<IActionResult> SynthesizeAudio(
        SynthesizeAudioCommand req,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(req, cancellationToken);
        if (result.IsFailure && result.Error.Code == Error.UnsupportedTTSLanguage.Code)
            return BadRequest(result.Error);
        if (result.IsFailure && result.Error.Code == Error.TextToSpeechGenerationError.Code)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}
