using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Users.GetUsers;
internal class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PaginatedResponse<UserShortResponse>>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IFileService _fileService;

    public GetUsersQueryHandler(IUserReadRepository userReadRepository, IFileService fileService)
    {
        _userReadRepository = userReadRepository;
        _fileService = fileService;
    }

    public async Task<Result<PaginatedResponse<UserShortResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _userReadRepository.GetUsersAsync(request.Filter, cancellationToken);
        foreach (var userShortResponse in result.Items)
        {
            if (userShortResponse.ImageKey != null)
                userShortResponse.ImageLink = await
                    _fileService.GetPresignedUrlForGetAsync(userShortResponse.ImageKey, cancellationToken);
        }
        return result;
    }
}
