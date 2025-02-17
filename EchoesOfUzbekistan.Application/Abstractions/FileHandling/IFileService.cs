using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Abstractions.FileHandling;
public interface IFileService
{
    Task<string> GetPresignedUrlForGetAsync(string key, CancellationToken cancellationToken = default);
    Task<string> GetPresignedUrlForPutAsync(string fileName, string filePath, string contentType, CancellationToken cancellationToken = default);
    Task DeleteAsync(string key, CancellationToken cancellationToken = default);
}
