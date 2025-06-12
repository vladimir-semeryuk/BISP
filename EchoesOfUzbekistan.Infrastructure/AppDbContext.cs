using EchoesOfUzbekistan.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EchoesOfUzbekistan.Infrastructure;
public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
