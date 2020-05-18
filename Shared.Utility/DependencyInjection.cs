using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Utility.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Utility
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddSecurityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAesCryptoEngine, AesCryptoEngine>();
            return services;
           
        }
    }
}
