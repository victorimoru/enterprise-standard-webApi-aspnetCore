using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.Core.Filters
{
    public class CheckPermissionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var headerValue = context.HttpContext.Request.Headers["permission"];
            if (!headerValue.Equals("Si3plePassw0rd"))
            {
                context.Result = new BadRequestObjectResult("Invalid permission Header");
            };
        }

    }
}
