namespace EchoesOfUzbekistan.Domain.Guides.Repositories;
public interface IGuideRepository
{
    Task<AudioGuide?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<AudioGuide>> GetAllAsync(CancellationToken cancellationToken);
    Task<IList<AudioGuide>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    void Add(AudioGuide guide);
    void Update(AudioGuide guide);
    void Delete(AudioGuide guide);
}
