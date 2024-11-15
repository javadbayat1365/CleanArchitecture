using Application.Common;
using Application.Contracts.User;
using Application.Contracts.User.Models;
using Application.Extensions;
using Mediator;

namespace Application.Features.User.Queries.PasswordLogin;

public class UserPasswordLoginQueryHandler(IUserManager userManager,IJwtService jwtService) : IRequestHandler<UserPasswordLoginQuery, OperationResult<JwtAccessTokenModel>>
{
    public async ValueTask<OperationResult<JwtAccessTokenModel>> Handle(UserPasswordLoginQuery request, CancellationToken cancellationToken)
    {
        var validator = new UserPasswordLoginQueryValidator();
        var validationResult =await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            return OperationResult<JwtAccessTokenModel>.FailureResult(validationResult.Errors.ConvertToKeyValuepair());

        var user = request.UserNameOrEmail.IsEmail()
            ? await userManager.GetUserByEmailAsync(request.UserNameOrEmail, cancellationToken)
            : await userManager.GetUserByUserNameAsync(request.UserNameOrEmail, cancellationToken);

        if (user is null)
            return OperationResult<JwtAccessTokenModel>.FailureResult(nameof(UserPasswordLoginQuery.UserNameOrEmail),"User Not Found!");

        var passwordValidation = await userManager.PasswordCreateAsync(user, cancellationToken);
        if(passwordValidation.Succeeded)
        {
            var JwtAccessTokenModel =await jwtService.GetJwtAccessTokenAsync(user,cancellationToken);
            return OperationResult<JwtAccessTokenModel>.SuccessResult(JwtAccessTokenModel);
        }

        return OperationResult<JwtAccessTokenModel>.FailureResult(passwordValidation.Errors.ConvertToKeyValuePair());
    }
}
