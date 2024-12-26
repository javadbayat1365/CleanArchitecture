using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.Location.Commands;

public record CreateLocationCommand(string LocationName)
       : IRequest<OperationResult<bool>>,
         IValidatableModel<CreateLocationCommand>
{
    public IValidator<CreateLocationCommand> Validate(ValidationModelBase<CreateLocationCommand> validator)
    {
        validator.RuleFor(r => r.LocationName).NotEmpty();
        return validator;
    }
}
