using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Users.Services;
using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Guides;
using EchoesOfUzbekistan.Domain.Guides.Repositories;

namespace EchoesOfUzbekistan.Application.AudioGuides.ConfirmPurchase;
internal class ConfirmPurchaseCommandHandler : ICommandHandler<ConfirmPurchaseCommand>
{
    private readonly IGuidePurchaseRepository _guidePurchaseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmPurchaseCommandHandler(IUserContextService userContextService, IGuidePurchaseRepository guidePurchaseRepository, IUnitOfWork unitOfWork)
    {
        _guidePurchaseRepository = guidePurchaseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ConfirmPurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchaseExists = await _guidePurchaseRepository.ExistsAsync(request.UserId, request.GuideId);

        if (purchaseExists)
        {
            return Result.Success(purchaseExists);
        }

        var purchase = new AudioGuidePurchase(request.UserId, request.GuideId);
        try
        {
            await _guidePurchaseRepository.AddAsync(purchase);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return Result.Failure(AudioGuideErrors.PurchaseError);
        }

        return Result.Success();
    }
}
