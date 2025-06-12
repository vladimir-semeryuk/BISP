using EchoesOfUzbekistan.Application.Files.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Infrastructure.FileUpload;
public class FileAuthorizationService : IFileAuthorizationService
{
    public Result EnsureUserOwnsFiles(IEnumerable<string> keys, Guid userId)
    {
        var expectedPrefix1 = $"files/{userId}/";
        var expectedPrefix2 = $"eou-test/files/{userId}";

        var unauthorized = keys
            .Where(k => !(k.StartsWith(expectedPrefix1, StringComparison.Ordinal) || k.StartsWith(expectedPrefix2, StringComparison.Ordinal)))
            .ToList();

        if (unauthorized.Any())
        {
            return Result.Failure(Error.UnauthorizedAccess);
        }

        return Result.Success();
    }
}
