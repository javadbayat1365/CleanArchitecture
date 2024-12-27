using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.Category.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid CategoryId) : IRequest<OperationResult<GetCategoryByIdQueryResult>>, IValidatableModel<GetCategoryByIdQuery>
{
    public IValidator<GetCategoryByIdQuery> Validate(ValidationModelBase<GetCategoryByIdQuery> validator)
    {
        validator.RuleFor(x => x.CategoryId).NotEmpty();
        return validator;
    }
}
