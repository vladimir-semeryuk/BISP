using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.AudioGuides.PurchaseAudioGuide;

public record PurchaseAudioGuideCommand(Guid GuideId) : ICommand<string>;
