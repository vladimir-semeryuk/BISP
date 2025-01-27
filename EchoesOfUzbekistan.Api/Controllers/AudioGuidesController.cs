﻿using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
using EchoesOfUzbekistan.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AudioGuidesController : ControllerBase
{
    private readonly ISender _sender;

    public AudioGuidesController(ISender sender)
    {
        _sender = sender;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAudioGuide(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAudioGuideQuery(id);

        Result<AudioGuideResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }
}
