using Application.Common;
using Application.Common.Validation;
using Application.Contracts.User.Models;
using FluentValidation;
using Mediator;

namespace Application.Features.User.Queries.PasswordLogin;

public record UserPasswordLoginQuery(string UserNameOrEmail, string Password)
    : IRequest<OperationResult<JwtAccessTokenModel>>, IValidatableModel<UserPasswordLoginQuery>
{
    public IValidator<UserPasswordLoginQuery> Validate(ValidationModelBase<UserPasswordLoginQuery> validator)
    {
       validator.RuleFor(x => x.Password).NotEmpty();
        validator.RuleFor(x => x.UserNameOrEmail).NotEmpty();
        return validator;
    }
}
