using EchoesOfUzbekistan.Application.Abstractions;
using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.TextToSpeech.SynthesizeAudio;

public record SynthesizeAudioCommand(string Text, string LanguageCode, EntityTypes EntityType) : ICommand<SynthesizeAudioResult>;
