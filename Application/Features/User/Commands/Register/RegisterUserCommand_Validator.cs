using FluentValidation;

namespace Application.Features.User.Commands.Register;

public class RegisterUserCommand_Validator :AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommand_Validator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Must(x => int.TryParse(x,out var val) && val>0);
            //.Matches(@"^\?[1-9]\d{1,14}$");//12345678 //TODO Handle PhoneNumber Validation
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.RepeatPasword)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Password and Repeat Password Are Not Same!");

    }
}
