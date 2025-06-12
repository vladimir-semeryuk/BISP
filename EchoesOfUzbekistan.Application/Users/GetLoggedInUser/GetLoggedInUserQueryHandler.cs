using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.GetUser;
using EchoesOfUzbekistan.Application.Users.Interfaces;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Users.GetLoggedInUser;

internal sealed class GetLoggedInUserQueryHandler
    : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IUserContextService _userContext;
    private readonly IFileService _fileService;

    public GetLoggedInUserQueryHandler(
        IUserContextService userContext, IFileService fileService, IUserReadRepository userReadRepository)
    {
        _userContext = userContext;
        _fileService = fileService;
        _userReadRepository = userReadRepository;
    }

    public async Task<Result<UserResponse>> Handle(
        GetLoggedInUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userReadRepository.GetByIdentityIdAsync(_userContext.IdentityId, cancellationToken);

        if (user == null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound);
        }

        if (user.ImageKey != null)
        {
            user.ImageLink = await _fileService.GetPresignedUrlForGetAsync(user.ImageKey, cancellationToken);
        }

        return user;
    }
}
