using Application.Common;
using Clean.WebFramework.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clean.WebFramework.Common;

public class BaseController:ControllerBase
{
    protected string? UserName =>  User.Identity?.Name;
    protected Guid? UserId => Guid.Parse(User.Identity?.GetUserId()!);
    protected string? UserEmail => User.Identity?.FindFirsValue(ClaimTypes.Email);
    protected string? UserKey => User.Identity?.FindFirsValue(ClaimTypes.UserData);

    public IActionResult OperationResult<Tmodel>(OperationResult<Tmodel> result)
    {
        if (result.IsSuccess)
        {
            return result.Result is bool ? Ok() : Ok(result.Result);
        }

        AddErrors(result);
        if (result.IsNotFound)
        {
            var notFoundErrors = new ValidationProblemDetails(ModelState);
            return NotFound(notFoundErrors);
        }

            var badRequestErrors = new ValidationProblemDetails(ModelState);
            return BadRequest(badRequestErrors);
        
    }

    private void AddErrors<Tmodel>(OperationResult<Tmodel> result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key,error.Value);
        }
    }
}
