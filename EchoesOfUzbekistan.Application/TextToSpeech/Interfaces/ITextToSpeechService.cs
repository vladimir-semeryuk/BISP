namespace EchoesOfUzbekistan.Application.TextToSpeech.Interfaces;
public interface ITextToSpeechService
{
    public string ContentType {
        get;
    }
    string? GetVoiceNameByLanguage(string languageCode);
    Task<byte[]?> SynthesizeSpeechAsync(string text, string languageCode);
}
