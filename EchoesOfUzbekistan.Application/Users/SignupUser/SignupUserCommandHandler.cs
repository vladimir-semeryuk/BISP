using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Users.SignupUser;
internal class SignupUserCommandHandler : ICommandHandler<SignupUserCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authenticationService;

    public SignupUserCommandHandler(
        IUnitOfWork unitOfWork, 
        IUserRepository userRepository,
        IAuthService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }

    public async Task<Result<Guid>> Handle(
        SignupUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = User.Create(
            new FirstName(request.FirstName),
            new Surname(request.Surname),
            new Email(request.Email),
            new Country(request.CountryName, request.CountryCode),
            string.IsNullOrWhiteSpace(request.City) ? null : new City(request.City));

        try
        {
            string identityProviderId = await _authenticationService.Signup(
                user,
                request.Password,
                cancellationToken);
            user.SetIdentityId(identityProviderId);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(UserErrors.InvalidCredentials);
        }

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
