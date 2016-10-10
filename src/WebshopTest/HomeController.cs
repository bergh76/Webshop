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
using System.Collections.Generic;
using System.Threading.Tasks;

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

        //private readonly IHostingEnvironment hostEnvironment;
        //private readonly IStringLocalizer<ArticleController> localizer;


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
        [Fact]
        public async Task TestSearchFunctionOnVendorIdInSearchBar()
        {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            int vendorID = 9001;
            int categoryID = 1001;
            int productID = 1010;
            int subProductID = 2010;
            string lang = "sv";

            // Insert seed data into the database using one instance of the context            
            using (var context = new WebShopRepository(options))
            {
                context.Vendors.Add(new VendorModel { ID = 1, VendorID = vendorID, VendorName = "Sony", LangCode = lang, ISActive = true, VendorWebPage = "http://www.asdf.com" });
                context.SaveChanges();
            }
            using (var context = new WebShopRepository(options))
            {
                context.Categories.Add(new CategoryModel { ID = 1, CategoryID = categoryID, CategoryName = "Ljud & Bild", LangCode = lang, ISActive = true });
                context.SaveChanges();
            }
            using (var context = new WebShopRepository(options))
            {
                context.Products.Add(new ProductModel { ID = 1, ProductID = productID, ProductName = "Hemmabio & HiFi", LangCode = lang, ISActive = true });
                context.SaveChanges();
            }
            using (var context = new WebShopRepository(options))
            {
                context.SubCategories.Add(new SubCategoryModel { ID = 1, SubCategoryID = subProductID, SubCategoryName = "7.2 Reciever", LangCode = lang, ISActive = true });
                context.SaveChanges();
            }
            using (var context = new WebShopRepository(options))
            {
                context.Images.Add(new ImageModel { ImageDate = DateTime.Now, ImageName = "TestImage.jpg", ImagePath = "/images/products/", ArtikelId = 1 });
                context.SaveChanges();
                Articles article = new Articles()
                {
                    ArticleId = 1,
                    ArticleNumber = "123456789",
                    ArticlePrice = 1234,
                    ArticleAddDate = DateTime.Now,
                    ArticleStock = 2,
                    VendorId = vendorID,
                    CategoryId = categoryID,
                    ProductId = productID,
                    SubCategoryId = subProductID,
                    ISActive = true,
                    ISCampaign = false,
                    _Category = null,
                    _Product = null,
                    _SubCategory = null,
                    _Vendor = null

                };
                context.Add(article);
                context.SaveChanges();
                ArticleTranslation artTrans = new ArticleTranslation()
                {
                    ArticleId = 1,
                    ArticleName = "Sony Flatscreen",
                    ArticleShortText = "ShortText",
                    ArticleFeaturesOne = "One",
                    ArticleFeaturesTwo = "Two",
                    ArticleFeaturesThree = "Three",
                    ArticleFeaturesFour = "Four",
                    LangCode = lang,
                    ISTranslated = true

                };
                context.Add(artTrans);
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WebShopRepository(options))
            {
                var service = new HomeController(null, null);
                //Act
                var query = await service.SearchArticles(context, vendorID, 0, 0, 0);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(query);
                var model = Assert.IsAssignableFrom<IEnumerable<ArticlesViewModel>>(
                    viewResult.ViewData.Model);
                Assert.Equal(1, model.Count());
                Assert.Equal(9001, model.ElementAt(0).VendorID);
                //Assert.Equal("Sony", model.ElementAt(0)._Vendor.VendorName);
                Assert.Contains("Sony Flatscreen", model.ElementAt(0).ArticleName);
            }
        }
    }
}
