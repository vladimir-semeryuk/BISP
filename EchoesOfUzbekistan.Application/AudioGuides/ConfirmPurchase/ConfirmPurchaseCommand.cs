using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.AudioGuides.ConfirmPurchase;

public record ConfirmPurchaseCommand(Guid UserId, Guid GuideId) : ICommand;
