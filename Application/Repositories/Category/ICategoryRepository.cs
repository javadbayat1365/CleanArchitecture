using Domain.Entities.Ad;

namespace Application.Repositories.Category;

public interface ICategoryRepository
{
    Task CreateAsync(CategoryEntity category,CancellationToken cancellationToken= default);
    Task<List<CategoryEntity>> GetCategoriesBaseOnNameAsync(string categoryName,CancellationToken cancellationToken = default);
    Task<CategoryEntity?> GetCategoryByIdAsync(Guid categoryId,CancellationToken cancellationToken = default);

}
