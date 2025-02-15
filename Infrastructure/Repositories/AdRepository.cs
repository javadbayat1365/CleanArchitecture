using Application.Repositories.Ad;
using Domain.Entities.Ad;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Common;

namespace Persistence.Repositories;

internal class AdRepository(CleanDbContext db) : BaseRepository<AdEntitiy>(db), IAdRepository
{
    public async Task CreateAdAsync(AdEntitiy adEntitiy, CancellationToken cancellationToken = default)
    {
       await  AddAsync(adEntitiy, cancellationToken);
    }

    public async Task<AdEntitiy?> GetAdByIdAsync(Guid adId, CancellationToken cancellationToken = default)
    {
        return await TableNoTracking.SingleOrDefaultAsync(s => s.Id == adId,cancellationToken);
    }

    public async Task<AdEntitiy?> GetAdByIdForUpdateAsync(Guid adId, CancellationToken cancellationToken = default)
    {
        return await Table.SingleOrDefaultAsync(s => s.Id.Equals(adId),cancellationToken);
    }

    public async Task<AdEntitiy?> GetAdDetailByIdAsync(Guid adId, CancellationToken cancellationToken)
    {
        return await TableNoTracking
            .Include(s => s.User)
            .Include(s => s.Location)
            .Include(s => s.Category)
            .SingleOrDefaultAsync(s => s.Id.Equals(adId), cancellationToken);
    }

    public async Task<List<AdEntitiy>> GetUserAdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await TableNoTracking.Where(s => s.UserId.Equals(userId)).ToListAsync(cancellationToken);
    }

    public async Task<List<AdEntitiy>> GetVerifiedAdsAsync(int currentPage, int pageCount, CancellationToken cancellationToken = default)
    {
        return await TableNoTracking
            .Where(s => s.CurrentState == AdEntitiy.AdState.Approved)
            .Skip((currentPage-1) * pageCount)
            .Take(pageCount)
            .ToListAsync(cancellationToken);
    }
}
