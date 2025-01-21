using Application.Repositories.Category;
using Domain.Entities.Ad;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Common;

namespace Persistence.Repositories;

internal class CategoryRepository(CleanDbContext db) : BaseRepository<CategoryEntity>(db), ICategoryRepository
{
    public async Task CreateAsync(CategoryEntity category, CancellationToken cancellationToken = default)
    {
        await base.AddAsync(category, cancellationToken);
    }

    public Task<List<CategoryEntity>> GetCategoriesBaseOnNameAsync(string categoryName, CancellationToken cancellationToken = default)
    {
        return TableNoTracking.Where(s => s.Name.Contains(categoryName)).ToListAsync(cancellationToken);
    }

    public Task<CategoryEntity?> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return base.TableNoTracking.SingleOrDefaultAsync(s => s.Id.Equals(categoryId),cancellationToken);
    }
}
