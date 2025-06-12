using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.AudioGuides.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.AudioGuides.GetGuidePurchaseStatusForUser;
internal class GetGuidePurchaseStatusForUserQueryHandler : IQueryHandler<GetGuidePurchaseStatusForUserQuery, bool>
{
    private readonly IGuidePurchaseReadRepository _guidePurchaseReadRepository;

    public GetGuidePurchaseStatusForUserQueryHandler(IGuidePurchaseReadRepository guidePurchaseReadRepository)
    {
        _guidePurchaseReadRepository = guidePurchaseReadRepository;
    }

    public async Task<Result<bool>> Handle(GetGuidePurchaseStatusForUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _guidePurchaseReadRepository.ExistsAsync(request.UserId, request.GuideId);
        return result;
    }
}
