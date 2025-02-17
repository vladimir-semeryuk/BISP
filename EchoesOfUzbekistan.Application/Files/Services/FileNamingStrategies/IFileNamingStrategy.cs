using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Files.Services.FileNamingStrategies;
public interface IFileNamingStrategy
{
    string GetFilePath(string userId, string fileId, string contentType);
}