using Application.Repositories.LocationRepository;

namespace Application.Repositories.Common;

public interface IUnitOfWork:IDisposable,IAsyncDisposable
{
    ILocationRepository LocationRepository { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
}
