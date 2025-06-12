using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Users.GetUser;
public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IFileService _fileService;

    public GetUserQueryHandler(IFileService fileService, IUserReadRepository userReadRepository)
    {
        _fileService = fileService;
        _userReadRepository = userReadRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userReadRepository.GetByIdAsync(request.userId, cancellationToken);
        if (user != null && user.ImageKey != null)
        {
            user.ImageLink = await _fileService.GetPresignedUrlForGetAsync(user.ImageKey, cancellationToken);
        }
        return user;
    }
}
