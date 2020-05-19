using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DatingApp.API
{
    public static class HttpContextExtension
    {
        public static (string errorMsg, string id, string Gender) ValidateUserWithJWTClaim(this HttpContext httpContext)
        {
            if (httpContext.User == null) return ("Invalid", null, null);
            var UserId = httpContext.User.Claims.SingleOrDefault(x => x.Type == "Id").Value;
            var Gender = httpContext.User.Claims.SingleOrDefault(x => x.Type == "Gender").Value;
            return (null, UserId, Gender);
            
        }
    }
}
