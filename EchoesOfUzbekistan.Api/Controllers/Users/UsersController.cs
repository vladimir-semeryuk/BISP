using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
using EchoesOfUzbekistan.Application.Users.GetLoggedInUser;
using EchoesOfUzbekistan.Application.Users.GetUser;
using EchoesOfUzbekistan.Application.Users.LoginUser;
using EchoesOfUzbekistan.Application.Users.SignupUser;
using EchoesOfUzbekistan.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers.Users;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();

        Result<UserResponse> result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(id);

        Result<UserResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> Signup(
        SignupUserRequest req,
        CancellationToken cancellationToken)
    {
        var command = new SignupUserCommand(
            req.Email,
            req.FirstName,
            req.Surname,
            req.Password,
            req.CountryName,
            req.CountryCode);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(request.Email, request.Password);

        Result<AccessTokenResponse> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }
}
