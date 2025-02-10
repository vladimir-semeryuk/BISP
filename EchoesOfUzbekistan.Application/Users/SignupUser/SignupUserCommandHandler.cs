using EchoesOfUzbekistan.Application.Abstractions.Auth;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Common;
using EchoesOfUzbekistan.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            new Country(request.CountryName, request.CountryCode));

        string identityProviderId = await _authenticationService.Signup(
            user,
            request.Password,
            cancellationToken);

        user.SetIdentityId(identityProviderId);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
