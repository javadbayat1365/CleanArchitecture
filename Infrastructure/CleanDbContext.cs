using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;

namespace Persistence;

public class CleanDbContext(DbContextOptions<CleanDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterEntities<IEntity>(typeof(IEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanDbContext).Assembly);
        modelBuilder.ApplyRestrictDeleteBehaviour();



        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyEntityChangeDates();
        return base.SaveChanges();
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyEntityChangeDates();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyEntityChangeDates();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyEntityChangeDates();
        return base.SaveChangesAsync(cancellationToken);
    }
    private void ApplyEntityChangeDates()
    {
        var entities = ChangeTracker.Entries()
            .Where(s => s is { Entity: IEntity, State: EntityState.Added } or { Entity: IEntity, State: EntityState.Modified });
        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                ((IEntity)entity.Entity).CreateDate = DateTime.Now;
            }
            else if(entity.State == EntityState.Modified)
            {
                ((IEntity)entity.Entity).ModifiedDate = DateTime.Now;
            }
        }
    }
}
