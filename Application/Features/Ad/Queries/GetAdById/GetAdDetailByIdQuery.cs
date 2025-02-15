using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.Ad.Queries.GetAdById;

public record GetAdDetailByIdQuery(Guid AdId) : IRequest<OperationResult<GetAdDetailByIdQueryResult>>, IValidatableModel<GetAdDetailByIdQuery>
{
    public IValidator<GetAdDetailByIdQuery> Validate(ValidationModelBase<GetAdDetailByIdQuery> validator)
    {
        validator.RuleFor(x=>x.AdId).NotEmpty();
        return validator;
    }
}
