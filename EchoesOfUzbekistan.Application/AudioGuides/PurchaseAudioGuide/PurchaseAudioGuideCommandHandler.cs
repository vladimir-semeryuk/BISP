using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Abstractions.Payments;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Guides.Repositories;

namespace EchoesOfUzbekistan.Application.AudioGuides.PurchaseAudioGuide;
public class PurchaseAudioGuideCommandHandler : ICommandHandler<PurchaseAudioGuideCommand>
{
    private readonly IGuideRepository _guideRepository;
    private readonly IGuidePurchaseRepository _purchaseRepository;
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IUserContextService _userContext;

    public PurchaseAudioGuideCommandHandler(IGuideRepository guideRepository, IGuidePurchaseRepository purchaseRepository, IPaymentProcessor paymentProcessor, IUserContextService userContext)
    {
        _guideRepository = guideRepository;
        _purchaseRepository = purchaseRepository;
        _paymentProcessor = paymentProcessor;
        _userContext = userContext;
    }

    public async Task<Result> Handle(PurchaseAudioGuideCommand request, CancellationToken cancellationToken)
    {
        var guide = await _guideRepository.GetByIdAsync(request.GuideId, cancellationToken);
        var userId = _userContext.UserId;
        if (guide == null)
            return Result.Failure(AudioGuideErrors.NotFound);

        if (guide.Price.Amount <= 0)
            return Result.Failure(AudioGuideErrors.FreeGuide);

        var hasAlreadyPurchased = await _purchaseRepository.ExistsAsync(userId, request.GuideId);
        if (hasAlreadyPurchased)
            return Result.Failure(AudioGuideErrors.AlreadyPurchased);

        var checkoutUrl = await _paymentProcessor.CreateCheckoutSessionAsync(guide, userId);
        return Result.Success(checkoutUrl);
    }
}
