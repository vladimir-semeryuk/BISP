using EchoesOfUzbekistan.Application.TextToSpeech.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;

namespace EchoesOfUzbekistan.Infrastructure.TextToSpeech;
internal class AzureTextToSpeechService : ITextToSpeechService
{
    private readonly IConfiguration _config;
    private readonly SpeechConfig _speechConfig;
    private readonly Dictionary<string, string> _voiceMappings;

    public AzureTextToSpeechService(IConfiguration config)
    {
        _config = config;
        _voiceMappings = _config.GetSection("Mappings")
                             .Get<Dictionary<string, string>>()
                         ?? throw new InvalidOperationException("Voice mappings not configured.");

        _speechConfig = SpeechConfig.FromSubscription(
            _config.GetSection("AzureTts:SubscriptionKey").Value,
            _config.GetSection("AzureTts:Region").Value);
        _speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Ogg24Khz16BitMonoOpus);
    }

    public string ContentType => "audio/ogg";

    public string? GetVoiceNameByLanguage(string languageCode)
    {
        if (_voiceMappings.TryGetValue(languageCode, out var voice))
            return voice;

        return null;
    }

    public async Task<byte[]?> SynthesizeSpeechAsync(string text, string languageCode)
    {
        var voiceName = GetVoiceNameByLanguage(languageCode);
        if (voiceName == null)
            throw new InvalidOperationException("Language isn't supported");
        _speechConfig.SpeechSynthesisVoiceName = voiceName;
        using var synthesizer = new SpeechSynthesizer(_speechConfig, null); // null is needed to indicate that
                                                                            // the service shouldn't play audio rn
        var result = await synthesizer.SpeakTextAsync(text);

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            return result.AudioData;
        else
            return null;
    }
}
