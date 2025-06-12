using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Files.Interfaces;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;

namespace EchoesOfUzbekistan.Application.Files.DeleteFile;
internal class DeleteFileCommandHandler : ICommandHandler<DeleteFileCommand>
{
    private readonly IUserContextService _userContextService;
    private readonly IFileAuthorizationService _fileAuthorizationService;
    private readonly IFileService _fileService;

    public DeleteFileCommandHandler(IFileService fileService, IUserContextService userContextService, IFileAuthorizationService fileAuthorizationService)
    {
        _fileService = fileService;
        _userContextService = userContextService;
        _fileAuthorizationService = fileAuthorizationService;
    }

    public async Task<Result> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userContextService.UserId;
            var ownsFile = _fileAuthorizationService.EnsureUserOwnsFiles(new List<string>{request.FileKey}, userId);
            if (ownsFile.IsFailure)
            {
                var allowedRoles = new List<string> { Role.Moderator.Name, Role.Administrator.Name };

                if (!_userContextService.Roles.Any(role => allowedRoles.Contains(role)))
                {
                    return Result.Failure(Error.UnauthorizedAccess);
                }
            }
            await _fileService.DeleteAsync(request.FileKey, cancellationToken);
        }
        catch (Exception e)
        {
            return Result.Failure(new Error("File.DeleteError", $"File couldn't be deleted. {e.Message}"));
        }

        return Result.Success();
    }
}
