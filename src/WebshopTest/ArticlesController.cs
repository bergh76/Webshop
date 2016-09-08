using Xunit;
using Microsoft.EntityFrameworkCore;
using Webshop.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Webshop.Controllers
{
    public class ArticlesControllerTest

    {
        private readonly IHostingEnvironment hostEnvironment;
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
        public async Task IndexListAllArticles()
        {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            using (var context = new WebShopRepository(options))
            {
                context.Articles.Add(new ArticleModel { ArticleName = "Test Product 1" });
                context.Articles.Add(new ArticleModel { ArticleName = "Test Product 2" });
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WebShopRepository(options))
            {
                var service = new ArticleController(context, hostEnvironment);
                //Act
                var result = await service.Index();
                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<ArticleModel>>(
                    viewResult.ViewData.Model);
                Assert.Equal(2, model.Count());
                Assert.Equal("Test Product 1", model.ElementAt(0).ArticleName);
            }
        }

        [Fact]
        public void CheckModelStateIsValid()
        {
            //Arrange
            var options = CreateNewContextOptions();
            using (var context = new WebShopRepository(options))
            {
                context.Articles.Add(
                    new ArticleModel
                    {
                        ArticleName = "Test Product 1",
                        ArticleAddDate = new System.DateTime(1999, 10, 1),
                    });
                var controller = new ArticleController(context, hostEnvironment);
                controller.ModelState.AddModelError("Error", "Error");
                //Act
                var result = controller.Index();
                //Assert
                var resultView = controller.ModelState.IsValid;
                Assert.Equal(resultView, false);
            }
        }

        [Fact]
        public void CheckRedirectionAfterCreation()
        {
            //Arrange
            var options = CreateNewContextOptions();
            using (var context = new WebShopRepository(options))
            {
                context.Products.Add(
                    new ProductModel
                    {
                        ProductName = "asfdadf",
                        ProductID = "9001",
                        ISActive = true
                    });
                context.SaveChanges();
            }
            // Use a clean instance of the context to run the test
            using (var context = new WebShopRepository(options))
            {
                var service = new ArticleController(context, hostEnvironment);
                //Act
                var result = service.NewProduct();
                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(1, context.Articles.Count());
            }
        }

        [Fact]
        public async Task DeleteArticlesFromDB()
        {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            using (var context = new WebShopRepository(options))
            {
                context.Articles.Add(new ArticleModel { ArticleName = "Test Product 1" });
                context.Articles.Add(new ArticleModel { ArticleName = "Test Product 2" });
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WebShopRepository(options))
            {
                var service = new ArticleController(context, hostEnvironment);
                //Act
                var result = await service.DeleteConfirmed(1);
                //Assert
                Assert.Equal(1, context.Articles.Count());
            }
        }

        [Fact]
        public async Task EditArticlesIdDB()
        {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            using (var context = new WebShopRepository(options))
            {
                context.Articles.Add(new ArticleModel
                {
                    ArticleID = 1,
                    ArticleName = "ProductName 1",
                    ArticleAddDate = new System.DateTime(2000, 1, 1),
                    ArticleFeaturesFour = "Blalalal",
                    ArticleFeaturesOne = "asdfasdf",
                    ArticleFeaturesThree = "asdfasdf",
                    ArticleFeaturesTwo = "asdfadsf",
                    ArticleGuid = "987638c9-50eb-42f2-81a6-a519439d168f",
                    ArticleImgPath = "",
                    ArticleNumber = "9999000222002",
                    ArticlePrice = 2999,
                    ArticleShortText = "baflbkj abfb abfbsfb",
                    ArticleStock = 3,
                    ISActive = true,
                    ISCampaign = false,
                    CategoryID = 1001,
                    ProductID = 111,
                    SubCategoryID = 999,
                    VendorID = 9111,
                    ProductImgPathID = 1
                });
                context.Articles.Add(new ArticleModel
                {
                    ArticleID = 2,
                    ArticleName = "ProductName 2",
                    ArticleAddDate = new System.DateTime(2000, 1, 1),
                    ArticleFeaturesFour = "Blalalal",
                    ArticleFeaturesOne = "asdfasdf",
                    ArticleFeaturesThree = "asdfasdf",
                    ArticleFeaturesTwo = "asdfadsf",
                    ArticleGuid = "987638c9-50eb-42f2-81a6-a519439d168f",
                    ArticleImgPath = "",
                    ArticleNumber = "9999000222002",
                    ArticlePrice = 2999,
                    ArticleShortText = "baflbkj abfb abfbsfb",
                    ArticleStock = 3,
                    ISActive = true,
                    ISCampaign = false,
                    CategoryID = 1001,
                    ProductID = 111,
                    SubCategoryID = 999,
                    VendorID = 9111,
                    ProductImgPathID = 1
                });
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WebShopRepository(options))
            {
                var service = new ArticleController(context, hostEnvironment);
                //Act
                var result = await service.Edit(1,
                    new ArticleModel
                    {
                        ArticleID = 1,
                        ArticleName = "Changed Name",
                        ArticleAddDate = new System.DateTime(2000, 1, 1),
                        ArticleFeaturesFour = "Blalalal",
                        ArticleFeaturesOne = "asdfasdf",
                        ArticleFeaturesThree = "asdfasdf",
                        ArticleFeaturesTwo = "asdfadsf",
                        ArticleGuid = "987638c9-50eb-42f2-81a6-a519439d168f",
                        ArticleImgPath = "",
                        ArticleNumber = "9999000222002",
                        ArticlePrice = 2999,
                        ArticleShortText = "baflbkj abfb abfbsfb",
                        ArticleStock = 3,
                        ISActive = true,
                        ISCampaign = false,
                        CategoryID = 1001,
                        ProductID = 111,
                        SubCategoryID = 999,
                        VendorID = 9111,
                        ProductImgPathID = 1
                    });

                //Assert

                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var dbCheck = context.Articles.SingleOrDefault(x => x.ArticleID == 1).ArticleName;

                Assert.Equal("Changed Name", dbCheck );
            }
        }

        [Fact]
        public async Task SearchFunction()
        {
            string search = "Sony";
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            using (var context = new WebShopRepository(options))
            {
                context.Articles.Add(new ArticleModel
                {
                    ArticleID = 1,
                    ArticleName = "Sony",
                    ArticleAddDate = new System.DateTime(2000, 1, 1),
                    ArticleFeaturesFour = "Blalalal",
                    ArticleFeaturesOne = "asdfasdf",
                    ArticleFeaturesThree = "asdfasdf",
                    ArticleFeaturesTwo = "asdfadsf",
                    ArticleGuid = "987638c9-50eb-42f2-81a6-a519439d168f",
                    ArticleImgPath = "",
                    ArticleNumber = "9999000222002",
                    ArticlePrice = 2999,
                    ArticleShortText = "baflbkj abfb abfbsfb",
                    ArticleStock = 3,
                    ISActive = true,
                    ISCampaign = false,
                    CategoryID = 1001,
                    ProductID = 111,
                    SubCategoryID = 999,
                    VendorID = 9111,
                    ProductImgPathID = 1
                });
                context.Articles.Add(new ArticleModel
                {
                    ArticleID = 2,
                    ArticleName = "Adidas",
                    ArticleAddDate = new System.DateTime(2000, 1, 1),
                    ArticleFeaturesFour = "Blalalal",
                    ArticleFeaturesOne = "asdfasdf",
                    ArticleFeaturesThree = "asdfasdf",
                    ArticleFeaturesTwo = "asdfadsf",
                    ArticleGuid = "987638c9-50eb-42f2-81a6-a519439d168f",
                    ArticleImgPath = "",
                    ArticleNumber = "9999000222002",
                    ArticlePrice = 2999,
                    ArticleShortText = "baflbkj abfb abfbsfb",
                    ArticleStock = 3,
                    ISActive = true,
                    ISCampaign = false,
                    CategoryID = 1001,
                    ProductID = 111,
                    SubCategoryID = 999,
                    VendorID = 9111,
                    ProductImgPathID = 1
                });
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WebShopRepository(options))
            {
                var service = new ArticleController(context, hostEnvironment);
                //Act
                var query = await service.Search(search);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(query);
                var model = Assert.IsAssignableFrom<IEnumerable<ArticleModel>>(
                    viewResult.ViewData.Model);
                Assert.Equal(1, model.Count());
            }
        }
    }
}