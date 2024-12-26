using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.Location.Queries.GetLocationById;

public record GetLocationByIdQuery(Guid LocationId)
    : IRequest<OperationResult<GetLocationByIdQueryResult>>, IValidatableModel<GetLocationByIdQuery>
{
    public IValidator<GetLocationByIdQuery> Validate(ValidationModelBase<GetLocationByIdQuery> validator)
    {
        validator.RuleFor(c => c.LocationId)
            .NotEmpty();
        return validator;
    }
}
