using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clean.WebFramework.Filters;

public class ModelStateValidationAttribute:ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var actionArgument in context.ActionArguments.Values)
        {
            var validator = context.HttpContext.RequestServices.GetService(typeof(IValidator<>).MakeGenericType(actionArgument!.GetType()));
            if(validator is IValidator validatorInstance)
            {
                var validationResult = await validatorInstance.ValidateAsync(new ValidationContext<object>(actionArgument));
                if(!validationResult.IsValid)
                    validationResult.Errors.ForEach(e => context.ModelState.AddModelError(e.PropertyName,e.ErrorMessage));
            }
        }
    }
}
