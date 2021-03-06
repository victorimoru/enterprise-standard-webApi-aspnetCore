using DatingApp.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.GlobalErrorHandler.Utility;
using Shared.Infrastructure;
using Shared.Infrastructure.LoggingHandler;
using Shared.Utility;
using System;
using System.IO;
using System.Reflection;
using System.Text;

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
            services.AddControllers().AddXmlDataContractSerializerFormatters();

            services.AddCors(options =>
            {
                options.AddPolicy("MyCustomPolicy", opt =>
                {
                    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(cfg => {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            }); 
            services
                .AddInfrastructure(Configuration)
                .AddBusinessServices(Configuration)
                .AddSecurityInfrastructure(Configuration)
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

            app.UseCors("MyCustomPolicy");

            app.UseAuthentication();

             app.UseRouting();
    

            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
