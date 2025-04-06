using Clean.WebFramework.Common;
using Microsoft.AspNetCore.Mvc;

namespace Clean.WebFramework.Controllers.V1;

public class UserController:BaseController
{
    [HttpGet]
    public virtual IActionResult GetName()
    {
        return Ok("بسیار عالی ورژن اول");
    }
}
