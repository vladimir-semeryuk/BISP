﻿using EchoesOfUzbekistan.Application.Abstractions.FileHandling;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Files.UploadFile;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Files.GetFile;
internal class GetFileCommandHandler : ICommandHandler<GetFileCommand, string>
{
    private readonly IFileService _fileService;

    public GetFileCommandHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<Result<string>> Handle(GetFileCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var url = await _fileService.GetPresignedUrlForGetAsync(request.key, cancellationToken);
            return Result.Success(url);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(new Error("File.Upload", $"Error retrieving presigned URL. \n{ex.Message}"));
        }
    }
}
