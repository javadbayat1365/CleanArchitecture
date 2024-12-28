using Application.Common;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests;

public static class Helpers
{
    public static ValueTask<TResponse> ValidateAndExcuteAsync<TRequest, TResponse>(TRequest request, IRequestHandler<TRequest, TResponse> handler, IServiceProvider serviceProvider)
    where TRequest : IRequest<TResponse> 
    where TResponse : IOperatoinResult, new()
    {
        var validator = serviceProvider.GetRequiredService<IValidator<TRequest>>();

        var validateRequestBehavior = new ValidateRequestBehavior<TRequest, TResponse>(validator);

        return validateRequestBehavior.Handle(request,CancellationToken.None,handler.Handle);
    }
}
