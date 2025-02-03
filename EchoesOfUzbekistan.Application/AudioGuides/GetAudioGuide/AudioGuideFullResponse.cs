using EchoesOfUzbekistan.Application.Places.GetPlace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
public class AudioGuideResponse : AudioGuideBaseResponse
{
    public ICollection<PlaceResponse> Places { get; init; } = new List<PlaceResponse>();
    public ICollection<AudioGuideTranslationResponse> Translations { get; init; } = new List<AudioGuideTranslationResponse>();
}

