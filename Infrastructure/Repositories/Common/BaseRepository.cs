using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Persistence.Repositories.Common;

internal abstract class BaseRepository<TEntity>(CleanDbContext db) where TEntity : class,IEntity
{
    private readonly DbSet<TEntity> _entities = db.Set<TEntity>();
    private protected IQueryable<TEntity> Table=>_entities.AsQueryable();
    private protected IQueryable<TEntity> TableNoTracking => _entities.AsNoTracking();

    protected virtual async ValueTask AddAsync(TEntity entity,CancellationToken token=default) => await _entities.AddAsync(entity,token);
    
    protected virtual async Task<List<TEntity>> GetAllAsync(CancellationToken token=default) => await _entities.ToListAsync(token);

    protected virtual async Task UpdateAsync(Expression<Func<TEntity, bool>> whereClause
        , Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> updateClause)
    => await _entities.Where(whereClause).ExecuteUpdateAsync(updateClause);

    protected virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> deleteClause,CancellationToken token)
        => await _entities.Where(deleteClause).ExecuteDeleteAsync(token);
    

}
