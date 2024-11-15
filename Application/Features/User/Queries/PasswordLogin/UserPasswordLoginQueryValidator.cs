using FluentValidation;

namespace Application.Features.User.Queries.PasswordLogin;

public class UserPasswordLoginQueryValidator:AbstractValidator<UserPasswordLoginQuery>
{
    public UserPasswordLoginQueryValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.UserNameOrEmail).NotEmpty();
    }
}
