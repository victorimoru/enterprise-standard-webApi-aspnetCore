using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure.DatabaseConnection;
using Shared.Infrastructure.LoggingHandler;
using Shared.Infrastructure.Repository;
using System.Threading.Tasks;

namespace DatingApp.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await InitializeDatabase(host);
            host.Run();
        }

        private static async Task InitializeDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var repoWrapper = services.GetRequiredService<IRepositoryWrapper>();
                var config = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILoggerManager>();
                try
                {
                    await new Seed(repoWrapper, config).SeedDataAsync();
                }
                catch (System.Exception)
                {

                    logger.LogError("An error occured during the initialization of Database");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
