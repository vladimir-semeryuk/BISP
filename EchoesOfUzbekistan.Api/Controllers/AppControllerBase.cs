using EchoesOfUzbekistan.Application.Users.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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