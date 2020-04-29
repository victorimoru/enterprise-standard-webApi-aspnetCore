using DatingApp.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared.GlobalErrorHandler.Utility;
using Shared.Infrastructure;
using Shared.Infrastructure.LoggingHandler;
using System;
using System.IO;
using System.Reflection;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.AddNLogConfigPATH();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("DatingAppOpenAPISpecification", new OpenApiInfo
                {
                    Title = "DatingApp API",
                    Version = "v1",
                    Description = "An API to perform DatingApp operations",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "IMORU Victor",
                        Email = "victorimoru23@gmail.com",
                        Url = new Uri("https://facebook.com/diadem2012"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "DatingApp API LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                      options.IncludeXmlComments(xmlPath);
            
            });
            services.AddControllers();
            services
                .AddInfrastructure(Configuration)
                .AddBusinessServices(Configuration)
            .ConfigureLoggerService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseApiExceptionHandler();

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/DatingAppOpenAPISpecification/swagger.json", "My DatingApp API V1");
                c.RoutePrefix = string.Empty;
            });

            // app.UseStaticFiles();

             app.UseRouting();
    

            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
