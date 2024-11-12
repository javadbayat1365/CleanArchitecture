using Application.Common;
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
    ):IRequest<OperationResult<bool>>;