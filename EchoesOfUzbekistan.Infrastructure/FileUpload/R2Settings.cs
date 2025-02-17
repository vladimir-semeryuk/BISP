using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.FileUpload;
public class R2Settings
{
    public string Region { get; init; } = "auto";
    public string BucketName { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretAccessKey { get; init; } = string.Empty;
    public string ServiceUrl { get; init; } = string.Empty;
}
