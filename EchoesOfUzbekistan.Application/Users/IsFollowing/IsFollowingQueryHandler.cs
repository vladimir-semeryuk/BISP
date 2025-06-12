using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Interfaces;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Users.IsFollowing;
internal class IsFollowingQueryHandler : IQueryHandler<IsFollowingQuery, bool>
{
    private readonly IUserContextService _userContextService;
    private readonly IUserReadRepository _userReadRepository;

    public IsFollowingQueryHandler(IUserReadRepository userReadRepository, IUserContextService userContextService)
    {
        _userReadRepository = userReadRepository;
        _userContextService = userContextService;
    }

    public async Task<Result<bool>> Handle(IsFollowingQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _userContextService.UserId;

        var result = await _userReadRepository.IsFollowingAsync(currentUserId, request.UserId);

        return result;
    }
}
