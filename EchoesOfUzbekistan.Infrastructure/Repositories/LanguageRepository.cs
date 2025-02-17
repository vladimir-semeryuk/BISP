using EchoesOfUzbekistan.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Repositories;
internal class LanguageRepository : Repository<Language>, ILanguageRepository
{
    public LanguageRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Language>> GetAllLanguagesAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<Language>().ToListAsync();
    }

    public new async Task<Language?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<Language>().FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Language?> GetLanguageByCode(string code, CancellationToken cancellationToken)
    {
        return await _context.Set<Language>().FirstOrDefaultAsync(l => l.Code == code);
    }
}
