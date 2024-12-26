using Application.Common.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationServiceCollectionExtionsions
{
    public static IServiceCollection RegisterApplicationValidators(this IServiceCollection services)
    {
        var validationTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(c => c.GetExportedTypes())
            .Where(w => w.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidatableModel<>)));

        foreach (var validationType in validationTypes)
        {
            var biggestConstrunctorLength = validationType.GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length).First().GetParameters().Length;

            var requestModel = Activator.CreateInstance(validationType, new object[biggestConstrunctorLength]);

            if (requestModel is null)
                continue;

            var requstMethodInfo = validationType.GetMethod(nameof(IValidatableModel<object>.Validate));

            var validationModelBase = Activator.CreateInstance(typeof(ValidationModelBase<>).MakeGenericType(validationType));

            if (validationModelBase is null)
                continue;

            var validator = requstMethodInfo?.Invoke(requestModel, [validationModelBase] );
            if (validator is null)
                continue;
            var validatorInterfaces = validator.GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));
            if (validatorInterfaces is null)
                continue;

            services.AddTransient(validatorInterfaces,_=> validator);
        }

        return services;
    }
}
