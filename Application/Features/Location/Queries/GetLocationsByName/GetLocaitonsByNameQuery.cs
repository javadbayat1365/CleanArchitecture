using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.Location.Queries.GetLocationsByName;

public record GetLocationsByNameQuery(string LocationName)
    : IRequest<OperationResult<IEnumerable<GetLocationsByNameQueryResult>>>, IValidatableModel<GetLocationsByNameQuery>
{
    public IValidator<GetLocationsByNameQuery> Validate(ValidationModelBase<GetLocationsByNameQuery> validator)
    {
        validator.RuleFor(c => c.LocationName)
            .NotEmpty()
            .MinimumLength(2);

        return validator;
    }
}
