using Microsoft.AspNetCore.Http;
using System;

namespace Shared.GlobalErrorHandler.Utility
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }
    }
}
