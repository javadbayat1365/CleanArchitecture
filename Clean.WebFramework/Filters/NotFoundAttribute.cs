using Microsoft.AspNetCore.Mvc.Filters;

namespace Clean.WebFramework.Filters;

public class NotFoundAttribute:ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        base.OnResultExecuting(context);
    }
}
