using Application.Common;
using Application.Repositories.Common;
using Mediator;

namespace Application.Features.Category.Queries.GetCategoriesByName;

public class GetCategoriesByNameQueryHandler (IUnitOfWork unitOfWork)
    : IRequestHandler<GetCategoriesByNameQuery, OperationResult<List<GetCategoriesByNameQueryResult>>>
{
    public async ValueTask<OperationResult<List<GetCategoriesByNameQueryResult>>> Handle(GetCategoriesByNameQuery request, CancellationToken cancellationToken)
    {
        var categories = await unitOfWork.CategoryRepository.GetCategoriesBaseOnNameAsync(request.CategoryName,cancellationToken);

        return OperationResult<List<GetCategoriesByNameQueryResult>>
            .SuccessResult(categories.Select(c => new GetCategoriesByNameQueryResult(c.Id, c.Name)).ToList());
    }
}
