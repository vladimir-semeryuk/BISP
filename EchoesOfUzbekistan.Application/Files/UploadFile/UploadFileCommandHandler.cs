using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Files.UploadFile;
internal class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, FileResponse>
{
    private readonly IFileService _fileService;
    private readonly IUserContextService _userContext;

    public UploadFileCommandHandler(IFileService fileService, IUserContextService userContextService)
    {
        _fileService = fileService;
        _userContext = userContextService;
    }

    public async Task<Result<FileResponse>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _userContext.UserId;
            
            Guid fileId = Guid.NewGuid();
            var strategy = FileNamingStrategyFactory.GetStrategy(request.EntityType);
            var filePath = strategy.GetFilePath(userId.ToString(), fileId.ToString(), request.ContentType);
            var putUrl = await _fileService.GetPresignedUrlForPutAsync(request.FileName, filePath, request.ContentType, cancellationToken);
            var getUrl = await _fileService.GetPresignedUrlForGetAsync(filePath, cancellationToken);
            return Result.Success(new FileResponse(putUrl, getUrl));
        }
        catch (Exception ex)
        {
            return Result.Failure<FileResponse>(new Error("File.Upload", $"Error retrieving presigned URL. \n{ex.Message}"));
        }
    }
}
