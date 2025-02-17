using EchoesOfUzbekistan.Application.Abstractions;
using EchoesOfUzbekistan.Application.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.Files.UploadFile;
public record UploadFileCommand(
    string userId,
    string FileName, 
    string ContentType, 
    EntityTypes EntityType) : ICommand<FileResponse>;
