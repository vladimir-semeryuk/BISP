﻿using EchoesOfUzbekistan.Application.Abstractions;
using EchoesOfUzbekistan.Application.Files.DeleteFile;
using EchoesOfUzbekistan.Application.Files.DeleteFiles;
using EchoesOfUzbekistan.Application.Files.GetFile;
using EchoesOfUzbekistan.Application.Files.UploadFile;
using EchoesOfUzbekistan.Application.Users.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers.Files;
[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IUserContextService _userContextService;

    public FilesController(ISender sender, IUserContextService userContextService)
    {
        _sender = sender;
        _userContextService = userContextService;
    }

    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(string fileName, string contentType, EntityTypes entityType, CancellationToken cancellationToken)
    {
        var userId = _userContextService.UserId.ToString();

        var command = new UploadFileCommand(userId, fileName, contentType, entityType);
        var response = await _sender.Send(command, cancellationToken);
        if (response.IsFailure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, (response.Error));
        }
        return Ok(response.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetFile(string key, string contentType, CancellationToken cts)
    {
        var command = new GetFileCommand(key, contentType);
        var response = await _sender.Send(command, cts);
        if (response.IsFailure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, (response.Error));
        }
        return Ok(response.Value);
    }

    [Authorize]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFiles([FromBody] DeleteFilesRequest request, CancellationToken cts)
    {
        var command = new DeleteFilesCommand(request.Keys);
        var result = await _sender.Send(command, cts);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteFile([FromQuery] string fileKey, CancellationToken cancellationToken)
    {
        var command = new DeleteFileCommand(fileKey);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
