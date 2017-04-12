using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SitePeinture.Dao;
using SitePeinture.Data;
using SitePeinture.Models;
using SitePeinture.Services;

namespace SitePeinture
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            configuration = builder.Build();
        }

        private IConfiguration configuration;

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = configuration.GetSection("connectionStrings:painting-local").Value;


            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IConfiguration>(_ => this.configuration);
            services.AddTransient<DaoPainting>();
            services.AddTransient<DaoTheme>();
            services.AddTransient<DaoEvent>();
            services.AddTransient<UserService>();
            services.AddTransient<MailService>();
            services.AddTransient<PaintingService>();
            services.AddMvc();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Check if the Db must be initialized
            DaoBase dao = new DaoBase(configuration);
            dao.Init();

            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Configure the HTTP request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseIdentity();
            app.UseMvcWithDefaultRoute();
        }
    }
}
