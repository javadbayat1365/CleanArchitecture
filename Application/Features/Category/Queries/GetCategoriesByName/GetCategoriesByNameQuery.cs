using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.Category.Queries.GetCategoriesByName;

public record GetCategoriesByNameQuery(string CategoryName) : IRequest<OperationResult<List<GetCategoriesByNameQueryResult>>>, IValidatableModel<GetCategoriesByNameQuery>
{
    public IValidator<GetCategoriesByNameQuery> Validate(ValidationModelBase<GetCategoriesByNameQuery> validator)
    {
        validator.RuleFor(x => x.CategoryName).NotEmpty();
        return validator;
    }
}
