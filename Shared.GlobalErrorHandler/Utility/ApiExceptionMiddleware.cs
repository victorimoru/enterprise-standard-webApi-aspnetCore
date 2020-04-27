using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared.Infrastructure.LoggingHandler;
using Shared.Utility;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Shared.GlobalErrorHandler.Utility
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILoggerManager loggerManager;
        private readonly ApiExceptionOptions options;

        public ApiExceptionMiddleware(RequestDelegate next, ILoggerManager loggerManager, ApiExceptionOptions options)
        {
            this.next = next;
            this.loggerManager = loggerManager;
            this.options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(httpContext, ex, options);
            }

        }

        private Task HandleExceptionAsync(HttpContext ctx, Exception ex, ApiExceptionOptions opts)
        {
            var error = new ApiError
            {
                Id = GuidGenerator.Generate(),
                StatusCode = (short)HttpStatusCode.InternalServerError,
                Title = "Internal Server Error"
            };
            opts.AddResponseDetails?.Invoke(ctx, ex, error);
            var innerExMessage = GetInnerMostExceptionMessage(ex);
            loggerManager.LogError($"{error.ToString()} \n{innerExMessage}");
            var result = JsonConvert.SerializeObject(error);
            // ctx.Response.AddApplicationError(error.Title);
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return ctx.Response.WriteAsync(error.ToString());

        }

        private string GetInnerMostExceptionMessage(Exception exception)
        {
            if (exception.InnerException != null)
                return GetInnerMostExceptionMessage(exception.InnerException);
            return exception.Message;
        }


    }

   
}
