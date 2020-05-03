using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.DatabaseConnection;
using Shared.Infrastructure.Repository;
using System;

namespace Shared.Infrastructure
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
       
            services.AddDbContext<DataContext>(opts =>
              opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
            return services;
        }
    }
}
