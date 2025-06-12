using EchoesOfUzbekistan.Application.Abstractions.Messages;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetGuidePurchaseStatusForUser;

public record GetGuidePurchaseStatusForUserQuery(Guid UserId, Guid GuideId) : IQuery<bool>;
