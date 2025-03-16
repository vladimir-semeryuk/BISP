using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace EchoesOfUzbekistan.Api.Controllers;

public abstract class AppControllerBase : ControllerBase
{
    protected readonly ISender _sender;
    protected readonly IUserContextService _userContextService;

    public AppControllerBase(ISender sender, IUserContextService userContextService)
    {
        _sender = sender;
        _userContextService = userContextService;
    }
    protected Guid CurrentUserId
    {
        get
        {
            var userId = _userContextService.UserId;
            return userId;
        }
    }
}