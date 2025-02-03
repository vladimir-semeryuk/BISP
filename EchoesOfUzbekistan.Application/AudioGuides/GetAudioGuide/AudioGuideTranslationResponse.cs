using EchoesOfUzbekistan.Domain.Guides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
public class AudioGuideTranslationResponse
{
    public Guid TranslationLanguageId { get; init; }
    public string Title { get; init; }
    public string? Descriptiopn { get; init; }
    public Guid AudioGuideId { get; init; }
}
