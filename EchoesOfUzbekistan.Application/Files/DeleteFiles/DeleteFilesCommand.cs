using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.Files.DeleteFiles;

public record DeleteFilesCommand(IEnumerable<string> keys) : ICommand;
