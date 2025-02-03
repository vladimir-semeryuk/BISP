using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;
public record AudioGuideFilter(
    Guid? CreatedByUserId = null,
    bool GetNewest = false,
    int? GetTopN = null
);