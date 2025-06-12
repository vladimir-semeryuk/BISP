using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.AudioGuides.DeleteAudioGuide;
public record DeleteAudioGuideCommand(Guid AudioGuideId) : ICommand;
