using EchoesOfUzbekistan.Application.Abstractions.Messages;
using EchoesOfUzbekistan.Application.Likes.Interfaces;
using EchoesOfUzbekistan.Domain.Abstractions;

namespace EchoesOfUzbekistan.Application.Likes.HasLiked;
internal class HasLikedQueryHandler : IQueryHandler<HasLikedQuery, bool>
{
    private readonly ILikeReadRepository _repository;

    public HasLikedQueryHandler(ILikeReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(HasLikedQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.HasUserLikedAsync(request.UserId, request.EntityId, request.EntityType, cancellationToken);
        return result;
    }
}
