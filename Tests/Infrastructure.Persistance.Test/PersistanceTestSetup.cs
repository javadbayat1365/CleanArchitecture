using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories.Common;
using Testcontainers.MsSql;

namespace Persistance.Test;

public class PersistenceTestSetup:IAsyncLifetime
{
    public UnitOfWork UnitOfWork { get; private set; }
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();//Hosted Db On Docker
    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var dbOptionBuilder = new DbContextOptionsBuilder<CleanDbContext>().UseSqlServer(_msSqlContainer.GetConnectionString());
        var db = new CleanDbContext(dbOptionBuilder.Options);

        await db.Database.MigrateAsync();
        UnitOfWork = new UnitOfWork(db);
    }
}
