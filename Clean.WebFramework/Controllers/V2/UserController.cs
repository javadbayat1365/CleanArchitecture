using Clean.WebFramework.Common;
using Microsoft.AspNetCore.Mvc;

namespace Clean.WebFramework.Controllers.V2;

public class UserController : V1.UserController
{
    [HttpGet]
    public override IActionResult GetName()
    {
        return Ok("بسیار عالی ورژن دوم");
    }
}
