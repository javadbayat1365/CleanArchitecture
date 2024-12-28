using Application.Repositories.Ad;
using Application.Repositories.Category;
using Application.Repositories.Location;
using System.Numerics;

namespace Application.Repositories.Common;

public interface IUnitOfWork:IDisposable,IAsyncDisposable
{
    ILocationRepository LocationRepository { get; }
    ICategoryRepository  CategoryRepository { get; }
    IAdRepository  AdRepository { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
}
