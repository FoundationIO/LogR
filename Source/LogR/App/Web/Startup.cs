using System.IO;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;
using Framework.Web.Filters;
using LogR.DI;
using LogR.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace LogR.Web
{
    public class Startup
    {
        private IHostingEnvironment hostingEnv;

        public Startup(IHostingEnvironment env)
        {
            hostingEnv = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                //set global format to json
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new GlobalExceptionFilter());
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddApplicationDI();

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "LogR Web",
                    Version = "1.0",
                    Description = "LogR Web"
                });

                if (hostingEnv.IsDevelopment())
                {
                    var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "LogR.Web.xml");
                    options.IncludeXmlComments(filePath);
                }

                options.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IBaseConfiguration config, ILog log)
        {
            loggerFactory.AddFrameworkLogger(config, log);

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseLogReceiverMiddleware();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger("swagger/{apiVersion}/swagger.json");
            app.UseSwaggerUi();
        }
    }
}
