using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Users.ChangePassword;
using EchoesOfUzbekistan.Application.Users.DeleteUser;
using EchoesOfUzbekistan.Application.Users.FollowUser;
using EchoesOfUzbekistan.Application.Users.GetFollowees;
using EchoesOfUzbekistan.Application.Users.GetFollowers;
using EchoesOfUzbekistan.Application.Users.GetLoggedInUser;
using EchoesOfUzbekistan.Application.Users.GetMutualFollows;
using EchoesOfUzbekistan.Application.Users.GetUser;
using EchoesOfUzbekistan.Application.Users.GetUsers;
using EchoesOfUzbekistan.Application.Users.IsFollowing;
using EchoesOfUzbekistan.Application.Users.LoginUser;
using EchoesOfUzbekistan.Application.Users.RefreshToken;
using EchoesOfUzbekistan.Application.Users.SignupUser;
using EchoesOfUzbekistan.Application.Users.UnfollowUser;
using EchoesOfUzbekistan.Application.Users.UpdateUser;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EchoesOfUzbekistan.Api.Controllers.Users;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ICookieService _cookieService;

    public UsersController(ISender sender, ICookieService cookieService)
    {
        _sender = sender;
        _cookieService = cookieService;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();

        Result<UserResponse> result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(id);

        Result<UserResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [Authorize(Roles = $"{Roles.Administrator},{Roles.Moderator}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);

        var result = await _sender.Send(command, cancellationToken);
        if (result.IsFailure && result.Error == Error.UnauthorizedAccess)
            return Forbid(result.Error.Name);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(filter);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserCommand command)
    {
        var result = await _sender.Send(command);
        if (result.Error == Error.UnauthorizedAccess)
            return Unauthorized(result);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
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
            req.CountryCode,
            req.City);

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

        Result<TokenResponse> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }

        _cookieService.SetAccessToken(result.Value.AccessToken, result.Value.ExpiresIn);
        _cookieService.SetRefreshToken(result.Value.RefreshToken, result.Value.RefreshExpiresIn);

        return Ok(new { message = "Logged in successfully" });
        // return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = _cookieService.GetRefreshToken();

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized(Result.Failure(UserErrors.InvalidRefreshToken));
        }

        var command = new RefreshTokenCommand(refreshToken);

        Result<TokenResponse> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return Unauthorized(result.Error);

        _cookieService.SetAccessToken(result.Value.AccessToken, result.Value.ExpiresIn);
        _cookieService.SetRefreshToken(result.Value.RefreshToken, result.Value.RefreshExpiresIn);

        return Ok(new { message = "Refreshed successfully" });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        _cookieService.ClearTokenCookies();
        return Ok();
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand(request.Email, request.OldPassword, request.NewPassword);

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure && result.Error == Error.UnauthorizedAccess)
            return Forbid();

        if (result.IsFailure)
            return BadRequest(result.Error);

;        return Ok();
    }

    [Authorize]
    [HttpPost("{id}/follow")]
    public async Task<IActionResult> Follow(Guid id)
    {
        var result = await _sender.Send(new FollowUserCommand(id));
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}/unfollow")]
    public async Task<IActionResult> Unfollow(Guid id)
    {
        var result = await _sender.Send(new UnfollowUserCommand(id));
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }

    [Authorize]
    [HttpGet("{id}/followers")]
    public async Task<IActionResult> GetFollowers(Guid id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _sender.Send(new GetFollowersQuery(id, pageNumber, pageSize));
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet("{id}/followees")]
    public async Task<IActionResult> GetFollowees(Guid id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _sender.Send(new GetFolloweesQuery(id, pageNumber, pageSize));
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet("{id}/mutuals")]
    public async Task<IActionResult> GetMutualFollows(Guid id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _sender.Send(new GetMutualFollowsQuery(id, pageNumber, pageSize));
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet("is-following/{id}")]
    public async Task<IActionResult> IsFollowing(Guid id)
    {
        var result = await _sender.Send(new IsFollowingQuery(id));
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }
}