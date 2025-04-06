using Clean.WebFramework.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clean.WebFramework.Filters;

public class ModelStateValidationAttribute:ActionFilterAttribute
{
    /// <summary>
    /// check for if modelstate is not valid
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
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

        if (!context.ModelState.IsValid)
        {
            var errors = new ValidationProblemDetails(context.ModelState);
            var apiResult = new ApiResult<IDictionary<string, string[]>>(false,ApiResultStatusCode.BadRequest.ToString(),ApiResultStatusCode.BadRequest,errors.Errors);
            context.Result = new JsonResult(apiResult) { StatusCode = StatusCodes.Status400BadRequest };
        }

        await base.OnActionExecutionAsync(context, next);
    }
}
