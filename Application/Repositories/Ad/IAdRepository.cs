using Domain.Entities.Ad;

namespace Application.Repositories.Ad;

public interface IAdRepository
{
    Task CreateAdAsync(AdEntitiy adEntitiy,CancellationToken cancellationToken=default);
    Task<AdEntitiy?> GetAdByIdAsync(Guid adId,CancellationToken cancellationToken = default);
    Task<AdEntitiy?> GetAdDetailByIdAsync(Guid adId,CancellationToken cancellationToken = default);
    Task<List<AdEntitiy>> GetUserAdsAsync(Guid userId,CancellationToken cancellationToken = default);
    Task<List<AdEntitiy>> GetVerifiedAdsAsync(int currentPage, int pageCount,CancellationToken cancellationToken = default);
    Task<AdEntitiy?> GetAdByIdForUpdateAsync(Guid adId, CancellationToken cancellationToken = default);
}
