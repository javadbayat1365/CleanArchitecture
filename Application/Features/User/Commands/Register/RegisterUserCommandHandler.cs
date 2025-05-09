﻿using Application.Common;
using Application.Contracts.User;
using Application.Extensions;
using Mediator;

namespace Application.Features.User.Commands.Register;

public sealed class RegisterUserCommandHandler(IUserManager userManager) : IRequestHandler<RegisterUserCommand, OperationResult<bool>>
{

    public async ValueTask<OperationResult<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        //var validator = new RegisterUserCommand_Validator();
        //var validationResult = await validator.ValidateAsync(request,cancellationToken);

        //if (!validationResult.IsValid)
        //    return OperationResult<bool>.FailureResult(validationResult.Errors.ConvertToKeyValuepair());

        var userCreateResult = await userManager.CreateByPasswordAsync(
            new Domain.Entities.User.UserEntity(
                request.FirstName,
                request.LastName,
                request.UserName,
                request.Email) { PhoneNumber = request.PhoneNumber },request.Password, cancellationToken);

        if (userCreateResult.Succeeded)
        {
            return OperationResult<bool>.SuccessResult(true);
            //TODO Send Confirmation Notification To User
        }

        return OperationResult<bool>.FailureResult(userCreateResult.Errors.ConvertToKeyValuePair());
    }
}
    