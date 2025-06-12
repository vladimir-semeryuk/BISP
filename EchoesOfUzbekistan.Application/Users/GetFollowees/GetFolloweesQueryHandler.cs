using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.GetFollowers;
using EchoesOfUzbekistan.Application.Users.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Users.GetFollowees;
internal class GetFolloweesQueryHandler : IQueryHandler<GetFolloweesQuery, PaginatedResponse<FriendResponse>>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IFileService _fileService;

    public GetFolloweesQueryHandler(IUserReadRepository userReadRepository, IFileService fileService)
    {
        _userReadRepository = userReadRepository;
        _fileService = fileService;
    }

    public async Task<Result<PaginatedResponse<FriendResponse>>> Handle(GetFolloweesQuery request, CancellationToken cancellationToken)
    {
        var result = await _userReadRepository.GetFolloweesAsync(request.Id, request.PageNumber, request.PageSize, cancellationToken);

        foreach (var friend in result.Items)
        {
            if (friend.ImageLink != null)
                friend.ImageLink =
                    await _fileService.GetPresignedUrlForGetAsync(friend.ImageLink, cancellationToken);
        }

        return result;
    }
}
