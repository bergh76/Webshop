using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Webshop.Interfaces;
using Webshop.Models;

namespace Webshop
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=Webshop;Trusted_Connection=True;";
            services.AddDbContext<WebShopRepository>(options => options.UseSqlServer(connection));
            // Add framework services.
            services.AddSingleton<IDateTime, SystemDateTime>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(); // Dependencyinjection

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "language",
                //    template: "{lang=se}/{controller=Home}/{action=About}/"); //direktsökning via urlsträngen

                //routes.MapRoute(
                //   name: "longroute",
                //   template: "Sverige/{controller}/{action}/{id}/{name}"); //direktsökning via urlsträngen

                //routes.MapRoute(
                //    name: "twoparameters",
                //    template: "{controller}/{action}/{id}/{name}"); //direktsökning via urlsträngen



            });
        }
    }
}
