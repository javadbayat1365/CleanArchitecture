using Clean.WebFramework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Clean.WebFramework.Filters;

public class BadRequestAttribute:ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not BadRequestObjectResult badRequestObjectResult)
            return;

        if(!context.ModelState.IsValid)
        {
            var errors = new ValidationProblemDetails(context.ModelState);

            var apiResult = new ApiResult<IDictionary<string, string[]>>(
                false,
                ApiResultStatusCode.BadRequest.ToString("G"),
                ApiResultStatusCode.BadRequest,
                errors.Errors);

            context.Result = new JsonResult(apiResult) { StatusCode = StatusCodes.Status400BadRequest };
            return;
        }
        var badReuestApiResult = new ApiResult<object>(false,
                ApiResultStatusCode.BadRequest.ToString("G"),
                ApiResultStatusCode.BadRequest,
                badRequestObjectResult.Value!);
            context.Result = new JsonResult(badReuestApiResult) { StatusCode = StatusCodes.Status400BadRequest };
    }
}
