using DatingApp.Core.Mapper;
using DatingApp.Core.ServiceContracts;
using DatingApp.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Core
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICustomMapper, CustomMapper>();
          
            return services;
        }
    }
}
