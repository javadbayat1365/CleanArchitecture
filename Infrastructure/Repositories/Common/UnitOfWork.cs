using Application.Repositories.Ad;
using Application.Repositories.Category;
using Application.Repositories.Common;
using Application.Repositories.Location;

namespace Persistence.Repositories.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly CleanDbContext _db;
    public UnitOfWork(CleanDbContext db)
    {
        _db = db;
        LocationRepository = new LocationRepository(db);
        CategoryRepository = new CategoryRepository(db);
        AdRepository = new AdRepository(db);
    }
    public ILocationRepository LocationRepository { get; }

    public ICategoryRepository CategoryRepository { get; }

    public IAdRepository AdRepository { get; }

    public async Task CommitAsync(CancellationToken cancellationToken = default) => await _db.SaveChangesAsync(cancellationToken);
    

    public void Dispose()
    {
        _db.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
       await _db.DisposeAsync();
    }
}
