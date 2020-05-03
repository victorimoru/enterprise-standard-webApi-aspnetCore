using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace DatingApp.Core.Filters
{
    public class CheckPermissionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var headerValue = context.HttpContext.Request.Headers["permission"];
            if (!headerValue.Equals("Si3plePassw0rd"))
            {
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Title = "Invalid Header/BadRequest",
                    Code = (int)HttpStatusCode.BadRequest,
                    Details = "checkPermission NOT found in the request header OR wrong value"
                }, Formatting.Indented);
                context.Result = new BadRequestObjectResult(result);
            };
        }

    }
}
