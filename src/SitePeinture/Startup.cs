using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.StaticFiles;
using Microsoft.Dnx.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using SitePeinture.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture
{
    public class Startup
    {
        public Startup(IApplicationEnvironment appEnv)
        {
            var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(appEnv.ApplicationBasePath)
                   .AddJsonFile("config.json");

            this.configuration = configurationBuilder.Build();
        }

        private IConfiguration configuration;

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConfiguration>(_ => this.configuration);
            services.AddTransient<DaoPainting>();
            services.AddTransient<DaoTheme>();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            // Configure the HTTP request pipeline.
            app.UseDefaultFiles(new DefaultFilesOptions() { DefaultFileNames = new[] { "index.html" } });
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
