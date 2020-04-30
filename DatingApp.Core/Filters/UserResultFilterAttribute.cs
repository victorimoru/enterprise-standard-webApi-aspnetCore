using DatingApp.Core.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Infrastructure.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Core.Filters
{
    public class UsersResultFilterAttribute : ResultFilterAttribute
    {
        private readonly ICustomMapper customMapper;

        public UsersResultFilterAttribute(ICustomMapper customMapper)
        {
            this.customMapper = customMapper;
        }
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

           if(typeof(IEnumerable).IsAssignableFrom(resultFromAction.Value.GetType()))
            {
                resultFromAction.Value = customMapper.MapToUserListDto((IEnumerable<User>)resultFromAction.Value);
            }
          
            await next();
        }
    }
}
