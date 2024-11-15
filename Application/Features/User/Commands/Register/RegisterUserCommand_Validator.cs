using FluentValidation;

namespace Application.Features.User.Commands.Register;

public class RegisterUserCommand_Validator :AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommand_Validator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).Equal(x => x.RepeatPasword).NotEmpty();

    }
}
