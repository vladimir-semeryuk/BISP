using EchoesOfUzbekistan.Domain.Guides.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoesOfUzbekistan.Domain.Guides;
using Microsoft.EntityFrameworkCore;

namespace EchoesOfUzbekistan.Infrastructure.Repositories;
public class GuidePurchaseRepository : IGuidePurchaseRepository
{
    private readonly AppDbContext _context;

    public GuidePurchaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid guideId)
    {
        return await _context.Set<AudioGuidePurchase>()
            .AnyAsync(p => p.UserId == userId && p.GuideId == guideId);
    }

    public async Task AddAsync(AudioGuidePurchase purchase)
    {
        _context.Set<AudioGuidePurchase>().Add(purchase);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Guid>> GetPurchasedGuideIdsAsync(Guid userId)
    {
        return await _context.Set<AudioGuidePurchase>()
            .Where(p => p.UserId == userId)
            .Select(p => p.GuideId)
            .ToListAsync();
    }
}

