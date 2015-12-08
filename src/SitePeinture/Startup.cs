using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.StaticFiles;
using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Models;
using SitePeinture.Dao;
using SitePeinture.Models;
using SitePeinture.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SitePeinture
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder()
                   .AddJsonFile("config.json")
                   .AddEnvironmentVariables();

            this.configuration = configurationBuilder.Build();
        }

        private IConfiguration configuration;

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = configuration.GetSection("connectionStrings:painting-local").Value;

            services.AddEntityFramework()
                .AddSqlite()
                .AddDbContext<IdentityContext>(options => options.UseSqlite(connection));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();

            services.AddTransient<IConfiguration>(_ => this.configuration);
            services.AddTransient<DaoPainting>();
            services.AddTransient<DaoTheme>();
            services.AddTransient<DaoEvent>();
            services.AddTransient<UserService>();
            services.AddTransient<MailService>();
            services.AddMvc();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Check if the Db must be initialized
            DaoBase dao = new DaoBase(configuration);
            dao.Init();

            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            // Configure the HTTP request pipeline.
            app.UseDefaultFiles(new DefaultFilesOptions() { DefaultFileNames = new[] { "index.html" } });
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseIdentity();
            app.UseMvc();
        }
    }
}
