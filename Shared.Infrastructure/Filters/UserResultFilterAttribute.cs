using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Filters
{
    public class UsersResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Headers.Add("OnResultExecuting",
           "This header was added by result filter.");

            var resultFromAction = context.Result as ObjectResult;
            if( resultFromAction?.Value == null || 
                resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }
          
            await next();
        }
    }
}
