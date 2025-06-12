using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Files.DeleteFile;
public record DeleteFileCommand(string FileKey) : ICommand;