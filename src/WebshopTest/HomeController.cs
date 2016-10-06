using Microsoft.AspNetCore.Mvc;
using System;
using Webshop.Interfaces;
using Webshop.ViewModels;
using System.Linq;
using Xunit;
using Webshop.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Webshop.Controllers
{
    //class StaticDateTime : IDateTime
    //{
    //    public DateTime Now {
    //        get {
    //            return new DateTime(2016, 09, 01, 6, 0, 0);
    //        }
    //    }
    //}
    public class HomeControllerTest
    {

        private readonly IHostingEnvironment hostEnvironment;
        private readonly IStringLocalizer<ArticleController> localizer;


        private static DbContextOptions<WebShopRepository> CreateNewContextOptions()
        {

            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<WebShopRepository>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
