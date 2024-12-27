using Application.Repositories.Category;
using Application.Repositories.Location;

namespace Application.Repositories.Common;

public interface IUnitOfWork:IDisposable,IAsyncDisposable
{
    ILocationRepository LocationRepository { get; }
    ICategoryRepository  CategoryRepository { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
}
