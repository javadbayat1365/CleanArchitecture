using Application.Common;
using Application.Repositories.Common;
using Domain.Entities.Ad;
using Mediator;

namespace Application.Features.Category.Commands.CreateCategory;

public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new CategoryEntity(request.categoryName);
        await unitOfWork.CategoryRepository.CreateAsync(category,cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return OperationResult<bool>.SuccessResult(true);
    }
}
