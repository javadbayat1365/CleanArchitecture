using Application.Common;
using Application.Contracts.User;
using Application.Extensions;
using Mediator;

namespace Application.Features.User.Commands.Register;

internal class RegisterUserCommandHandler(IUserManager userManager) : IRequestHandler<RegisterUserCommand, OperationResult<bool>>
{

    public async ValueTask<OperationResult<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new RegisterUserCommand_Validator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
            return OperationResult<bool>.FailureResult(validationResult.Errors.ConvertToKeyValuepair());

        var userCreateResult = await userManager.PasswordCreateAsync(
            new Domain.Entities.User.UserEntity(
                request.FirstName,
                request.LastName,
                request.UserName,
                request.Email) { PhoneNumber = request.PhoneNumber }, cancellationToken);

        if (userCreateResult.Succeeded)
        {
            return OperationResult<bool>.SuccessResult(true);
            //TODO Send Confirmation Notification To User
        }

        return OperationResult<bool>.FailureResult(userCreateResult.Errors.ConvertToKeyValuePair());
    }
}
