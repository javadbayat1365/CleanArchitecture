using Domain.Entities.Ad;

namespace Application.Repositories.LocationRepository;

public interface ILocationRepository
{
    Task CreateAsync(LocationEntity locationEntity,CancellationToken cancellationToken= default);
    Task<LocationEntity> GetLocationByIdAsync(Guid locationId,CancellationToken cancellationToken=default);
    Task<List<LocationEntity>> GetLocaitonsByNameAsync(string locationName,CancellationToken cancellationToken=default);
    Task<bool> IsLocationExistAsync(string locationName,CancellationToken cancellationToken = default);
}
