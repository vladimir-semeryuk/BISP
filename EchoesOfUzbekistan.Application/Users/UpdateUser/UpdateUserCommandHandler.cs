using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Users.UpdateUser;
internal class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var hasAccess = AuthorizationGuard.EnsureUserOwnsResource(_userContextService.UserId, request.UserId);
        if (hasAccess.IsFailure)
            return hasAccess;

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(UserErrors.NotFound);

        try
        {
            user.UpdateProfile(
                request.FirstName is not null ? new FirstName(request.FirstName) : null,
                request.Surname is not null ? new Surname(request.Surname) : null,
                request.AboutMe is null ? null : new AboutMe(request.AboutMe),
                request.CountryName is not null && request.CountryCode is not null
                    ? new Country(request.CountryName, request.CountryCode)
                    : null,
                request.City is not null ? new City(request.City) : null
            );
        }
        catch (ArgumentException ex)
        {
            var error = new Error("Validation.Failed", ex.Message);
            return Result.Failure(error);
        }

        if (user.ImageLink is not null && request.ImageLink is null)
        {
            var oldLink = user.ImageLink.Value;
            user.ClearProfilePicture();
        }
        else if (request.ImageLink is not null)
        {
            user.UpdateProfilePicture(new ResourceLink(request.ImageLink));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
