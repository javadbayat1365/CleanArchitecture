using Application.Common;
using Application.Repositories.Common;
using Mediator;

namespace Application.Features.Category.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCategoryByIdQuery, OperationResult<GetCategoryByIdQueryResult>>
{
    public async ValueTask<OperationResult<GetCategoryByIdQueryResult>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category =await unitOfWork.CategoryRepository.GetCategoryByIdAsync(request.CategoryId,cancellationToken);
        if (category is null)
            return OperationResult<GetCategoryByIdQueryResult>.NotFoundResult(
                nameof(GetCategoryByIdQuery.CategoryId),"Category Not Found!"
                );
        return OperationResult<GetCategoryByIdQueryResult>.SuccessResult(new GetCategoryByIdQueryResult(category.Id,category.Name));
    }
}
