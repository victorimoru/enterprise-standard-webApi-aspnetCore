using Microsoft.AspNetCore.Builder;
using System;

namespace Shared.GlobalErrorHandler.Utility
{
    public static class ApiExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder, Action<ApiExceptionOptions> configureOptins)
        {
            var options = new ApiExceptionOptions();
            configureOptins(options);
            return builder.UseMiddleware<ApiExceptionMiddleware>(options);
        }

        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
        {
            var options = new ApiExceptionOptions();
            return builder.UseMiddleware<ApiExceptionMiddleware>(options);
        }
    }
}
