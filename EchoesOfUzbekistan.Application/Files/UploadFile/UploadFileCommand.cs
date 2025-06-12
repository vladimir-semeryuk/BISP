using EchoesOfUzbekistan.Application.Abstractions;
using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Files.UploadFile;
public record UploadFileCommand(
    string userId,
    string FileName, 
    string ContentType, 
    EntityTypes EntityType) : ICommand<FileResponse>;
