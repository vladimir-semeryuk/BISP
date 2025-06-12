using EchoesOfUzbekistan.Application.Places.GetPlace;
using EchoesOfUzbekistan.Domain.Comments;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
public class AudioGuideResponse : AudioGuideBaseResponse
{
    public string AuthorName { get; init; }
    public ICollection<PlaceResponse> Places { get; init; } = new List<PlaceResponse>();
    public ICollection<AudioGuideTranslationResponse> Translations { get; init; } = new List<AudioGuideTranslationResponse>();
    public int LikeCount { get; init; }
    public string LanguageCode { get; init; }
}

