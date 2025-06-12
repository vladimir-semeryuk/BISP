using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Files.Interfaces;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Files.DeleteFiles;
internal class DeleteFilesCommandHandler : ICommandHandler<DeleteFilesCommand>
{
    private readonly IFileService _fileService;
    private readonly IFileAuthorizationService _fileAuthorizationService;
    private readonly IUserContextService _userContextService;

    public DeleteFilesCommandHandler(IFileService fileService, IUserContextService userContextService, IFileAuthorizationService fileAuthorizationService)
    {
        _fileService = fileService;
        _userContextService = userContextService;
        _fileAuthorizationService = fileAuthorizationService;
    }

    public async Task<Result> Handle(DeleteFilesCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.UserId;
        var authResult = _fileAuthorizationService.EnsureUserOwnsFiles(request.keys, userId);
        if (authResult.IsFailure)
            return authResult;

        await _fileService.DeleteBatchAsync(request.keys);
        return Result.Success();
    }
}
