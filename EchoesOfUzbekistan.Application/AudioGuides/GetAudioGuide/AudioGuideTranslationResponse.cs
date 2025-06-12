namespace EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
public class AudioGuideTranslationResponse
{
    public Guid TranslationLanguageId { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public Guid AudioGuideId { get; init; }
}
