using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure.DatabaseConnection;
using Shared.Infrastructure.LoggingHandler;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();
                var config = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILoggerManager>();
                context.Database.EnsureCreated();
                try
                {
                    new Seed(context, config).SeedData();
                }
                catch (System.Exception)
                {

                    logger.LogError("An error occured during the initialization of Database");
                }
            } 
                host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
