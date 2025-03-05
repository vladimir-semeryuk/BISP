using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Files.UploadFile;
internal class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, FileResponse>
{
    private readonly IFileService _fileService;
    private readonly IUserRepository _userRepository;

    public UploadFileCommandHandler(IFileService fileService, IUserRepository userRepository)
    {
        _fileService = fileService;
        _userRepository = userRepository;
    }

    public async Task<Result<FileResponse>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await _userRepository.GetByIdentityIdAsync(request.userId, cancellationToken);
            if (userId == null)
            {
                return Result.Failure<FileResponse>(UserErrors.NotFound);
            }
            
            Guid fileId = Guid.NewGuid();
            var strategy = FileNamingStrategyFactory.GetStrategy(request.EntityType);
            var filePath = strategy.GetFilePath(userId.Id.ToString(), fileId.ToString(), request.ContentType);
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
