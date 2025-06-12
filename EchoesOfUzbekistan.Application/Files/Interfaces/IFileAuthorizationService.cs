using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Files.Interfaces;
public interface IFileAuthorizationService
{
    Result EnsureUserOwnsFiles(IEnumerable<string> keys, Guid userId);
}