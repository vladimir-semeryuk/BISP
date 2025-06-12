using EchoesOfUzbekistan.Application.Abstractions.Data;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuide;
using EchoesOfUzbekistan.Application.AudioGuides.GetAudioGuides;

namespace EchoesOfUzbekistan.Application.AudioGuides.Interfaces;
public interface IGuideReadRepository
{
    Task<AudioGuideResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedResponse<AudioGuideShortResponse>> GetAudioGuidesAsync(AudioGuideFilter filter);
    Task<IEnumerable<AudioGuideShortResponse>> GetLikedGuidesAsync(Guid userId, int page, int pageSize);
}
