using Application.Common;
using Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Application.Features.User.Commands.Register;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string PhoneNumber,
    string Password,
    string RepeatPasword
    ) : IRequest<OperationResult<bool>>, IValidatableModel<RegisterUserCommand>
{
    public IValidator<RegisterUserCommand> Validate(ValidationModelBase<RegisterUserCommand> validator)
    {
        validator.RuleFor(x => x.Email).NotEmpty().EmailAddress();
        validator.RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Must(x => int.TryParse(x, out var val) && val > 0);
        //.Matches(@"^\?[1-9]\d{1,14}$");//12345678 //TODO Handle PhoneNumber Validation
        validator.RuleFor(x => x.FirstName).NotEmpty();
        validator.RuleFor(x => x.LastName).NotEmpty();
        validator.RuleFor(x => x.UserName).NotEmpty();
        validator.RuleFor(x => x.Password).NotEmpty();
        validator.RuleFor(x => x.RepeatPasword)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Password and Repeat Password Are Not Same!");
        return validator;
    }
}
