using EchoesOfUzbekistan.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Guides;
public interface IGuideRepository
{
    Task<AudioGuide?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<AudioGuide>> GetAllAsync(CancellationToken cancellationToken);
    void Add(AudioGuide guide);
    void Update(AudioGuide guide);
    void Delete(AudioGuide guide);
}
