using Application.Repositories.Location;
using Domain.Entities.Ad;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Common;

namespace Persistence.Repositories;

internal sealed class LocationRepository(CleanDbContext db) 
    : BaseRepository<LocationEntity>(db), ILocationRepository
{
    public async Task CreateAsync(LocationEntity locationEntity, CancellationToken cancellationToken = default)
    {

       await AddAsync(locationEntity,cancellationToken);
    }

    public async Task<List<LocationEntity>> GetLocationsByNameAsync(string locationName, CancellationToken cancellationToken = default)
    {
        return await TableNoTracking.Where(s => locationName.Contains(s.Name)).ToListAsync(cancellationToken);
    }

    public async Task<LocationEntity?> GetLocationByIdAsync(Guid locationId, CancellationToken cancellationToken = default)
        => await TableNoTracking.FirstOrDefaultAsync(w => w.Id.Equals(locationId), cancellationToken);

    public async Task<LocationEntity?> GetLocationByIdForEditAsync(Guid locationId, CancellationToken cancellationToken = default)
        => await Table.FirstOrDefaultAsync(w => w.Id.Equals(locationId), cancellationToken);

    public async Task<bool> IsLocationExistAsync(string locationName, CancellationToken cancellationToken = default)
    {
        return await TableNoTracking.AnyAsync(s => s.Name.Contains(locationName));
    }

    public async Task<List<LocationEntity>> GetLocaitonsByNameAsync(string locationName, CancellationToken cancellationToken = default)
    {
        return await TableNoTracking.Where(w => w.Name.Contains(locationName)).ToListAsync(cancellationToken);
    }
}
