using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.LoggingHandler
{
    public static class LogManagerExtension
    {
        public static void AddNLogConfigPATH(this IConfiguration configuration)
        {
            LogManager.LoadConfiguration(configuration["NLog:LogPATH"]);
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerService>();
        }
    }
}
