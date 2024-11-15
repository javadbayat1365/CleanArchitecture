using Application.Common;
using Application.Contracts.User.Models;
using Mediator;

namespace Application.Features.User.Queries.PasswordLogin;

public record UserPasswordLoginQuery(string UserNameOrEmail,string Password):IRequest<OperationResult<JwtAccessTokenModel>>;
