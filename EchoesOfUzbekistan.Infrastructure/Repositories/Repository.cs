using EchoesOfUzbekistan.Domain.Abstractions;
using EchoesOfUzbekistan.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoesOfUzbekistan.Infrastructure.Repositories;
internal abstract class Repository<T> where T : Entity
{
    protected readonly AppDbContext _context;

    protected Repository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public virtual void Add(T entity)
    {
        _context.Add(entity);
    }
}
