﻿using Localization.SqlLocalizer.DbStringLocalizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Webshop.Controllers;
using Webshop.Interfaces;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.Services;

namespace Webshop
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //var supported { }
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
            services.AddTransient<FixerIO>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddLocalization(option => option.ResourcesPath = "Resources");
            var connection = @"Server=(localdb)\mssqllocaldb;Database=Webshop;Trusted_Connection=True;";
            services.AddDbContext<WebShopRepository>(options => options.UseSqlServer(connection));
            // Add session related services.
            services.AddSession();

            // Add framework services.
            services.AddSingleton<IDateTime, SystemDateTime>();
            services.AddMvc()
                // Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                // Add support for localizing strings in data annotations (e.g. validation messages) via the
                // IStringLocalizer abstractions.
                .AddDataAnnotationsLocalization();
            // Configure supported cultures and localization options
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("sv-SE"),
                    new CultureInfo("en-GB")
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "sv-SE", uiCulture: "sv-SE");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Configure Session.
            app.UseSession();
            var supportedCultures = new[]
            {
                new CultureInfo("sv-SE"),
                new CultureInfo("en-GB")
            };
            //var localizationOptions = new RequestLocalizationOptions
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("sv-SE"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            //app.RequestCultureProviders.Insert(0, new HelperClasses.UrlCultureProvider());

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
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                //routes.MapRoute("areaRoute", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
                //routes.MapRoute(
                //    name: "lang",
                //    template: "{country}-{lang}/{controller}/{action}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute(
                //    name: "culture",
                //    template: "{language:regex(^[a-z]{{2}}(-[A-Z]{{2}})*$)}/{ controller = Home}/{ action = Index}/{ id ?}");
                //routes.MapRoute(
                //    name: "default", template: "{controller=Home}/{action=Index}/{id?}");


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