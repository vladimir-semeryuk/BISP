using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Domain.Common;
public interface ILanguageRepository
{
    Task<IEnumerable<Language>> GetAllLanguagesAsync(CancellationToken cancellationToken);
    Task<Language?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Language?> GetLanguageByCode(string code, CancellationToken cancellationToken);
}
