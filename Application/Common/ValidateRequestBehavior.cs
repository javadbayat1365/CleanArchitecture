using Application.Extensions;
using FluentValidation;
using Mediator;

namespace Application.Common;

public class ValidateRequestBehavior<TRequest, TResponse>(IValidator<TRequest> validator) : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse> 
       where TResponse : IOperatoinResult,new()
{
    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var validationResult = await validator.ValidateAsync(message,cancellationToken);
        if (!validationResult.IsValid)
        {
            return new TResponse()
            {
                IsSuccess = false,
                IsNotFound = false,
                ErrorMessages = validationResult.Errors.ConvertToKeyValuepair()
            };
        }
        return await next(message,cancellationToken);
    }
}
