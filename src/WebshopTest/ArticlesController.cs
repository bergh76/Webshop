// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;
using Webshop.ViewModels;
using Xunit;

namespace Webshop.Controllers
{
    public class ArticlesControllerTest

    {
        private readonly IHostingEnvironment hostEnvironment;
        private readonly IStringLocalizer<ArticleController> localizer;
        public static Guid _guid = new Guid("90b45d27-7767-4a77-8936-b1db338d3442");
    
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
        public void CreateData()
        {

        }
        [Fact]
        public async Task IndexListAllArticles()
        {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();
            string vendor = "Sony";
            string category = "Ljud & Bild";
            string product = "Hemmabio & HiFi";
            string subcategory = "7.2 Reciever";

            int vendorID = 9001;
            int categoryID = 1001;
            int productID = 1010;
            int subProductID = 2010;
            
            // Insert seed data into the database using one instance of the context            
            using (var context = new WebShopRepository(options))
            {
                context.Vendors.Add(new VendorModel { VendorID = vendorID, VendorName = "Sony" });
                context.SaveChanges();
            }
            using (var context = new WebShopRepository(options))
            {
                context.Categories.Add(new CategoryModel { CategoryID = categoryID, CategoryName = "Ljud & Bild" });
                context.SaveChanges();
            }
            using (var context = new WebShopRepository(options))
            {
                context.Products.Add(new ProductModel { ProductID = productID, ProductName = "Hemmabio & HiFi" });
                context.SaveChanges();
            }
            using (var context = new WebShopRepository(options))
            {
                context.SubCategories.Add(new SubCategoryModel { SubCategoryID = subProductID, SubCategoryName = "7.2 Reciever" });
                context.SaveChanges();
            }
            using (var context = new WebShopRepository(options))
            {
                context.Images.Add(new ImageModel
                {
                    ImageDate = DateTime.Now,
                    ImageName = "TestImage.jpg",
                    ImagePath = "/images/products/",
                    ArticleGuid = _guid
                    
                });
                context.SaveChanges();
                Articles article = new Articles()
                {
                    ArticleNumber = "123456789",
                    ArticlePrice = 1234,
                    ArticleAddDate = DateTime.Now,
                    ArticleStock = 2,
                    ImageId = 1,
                    VendorId = vendorID,
                    CategoryId = categoryID,
                    ProductId = productID,
                    SubCategoryId = subProductID,
                    ISActive = true,
                    ISCampaign = false,
                    ArticleGuid = _guid,
                    _Category = null,
                    _Image = null,
                    _Product = null,
                    _SubCategory = null,
                    _Vendor = null
                    
                };
                context.Add(article);
                context.SaveChanges();
                ArticleTranslation artTrans = new ArticleTranslation()
                {
                    ArticleName = "Test Product 1",
                    ArticleShortText = "ShortText",
                    ArticleFeaturesOne = "One",
                    ArticleFeaturesTwo = "Two",
                    ArticleFeaturesThree = "Three",
                    ArticleFeaturesFour = "Four",
                    LangCode = "sv",
                    ArticleNumber = "123456789",
                    ISTranslated = true
                    
                };
                context.Add(artTrans);
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WebShopRepository(options))
            {
                var service = new ArticleController(null, context, hostEnvironment, localizer);
                //Act
                var result = await service.Index(vendor, category, product, subcategory);
                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<ViewModels.ArticlesViewModel>>(
                    viewResult.ViewData.Model);
                Assert.Equal(1, model.Count());
                //Assert.Equal("Test Product 1", model.ElementAt(0).);
            }
        }

        //[Fact]
        //public void CheckModelStateIsValid()
        //{
        //    //Arrange
        //    var options = CreateNewContextOptions();
        //    string dropdownProduct = "";
        //    string dropdownSubCategory = "";
        //    string dropdownVendor = "";
        //    string dropdownCategory = "";
        //    int vendorID = 0;
        //    int productID = 0;
        //    string categoryID = "";
        //    int subProductID = 0;
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Articles.Add(
        //            new Articles
        //            {
        //                ArticleName = "Test Product 1",
        //                ArticleAddDate = new System.DateTime(1999, 10, 1),
        //            });
        //        var controller = new ArticleController(context, hostEnvironment, localizer);
        //        controller.ModelState.AddModelError("Error", "Error");
        //        //Act
        //        var result = controller.Index(dropdownCategory, dropdownProduct, dropdownSubCategory, dropdownVendor, vendorID, productID, categoryID, subProductID);
        //        //Assert
        //        var resultView = controller.ModelState.IsValid;
        //        Assert.Equal(resultView, false);
        //    }
        //}

        //[Fact]
        //public void CheckRedirectionAfterCreation()
        //{
        //    //Arrange
        //    var options = CreateNewContextOptions();
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Products.Add(
        //            new ProductModel
        //            {
        //                ProductName = "asfdadf",
        //                ProductID = "9001",
        //                ISActive = true
        //            });
        //        context.SaveChanges();
        //    }
        //    // Use a clean instance of the context to run the test
        //    using (var context = new WebShopRepository(options))
        //    {
        //        var service = new ArticleController(context, hostEnvironment, localizer);
        //        //Act
        //        var result = service.NewProduct();
        //        //Assert
        //        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        //        Assert.Equal(1, context.Articles.Count());
        //    }
        //}

        [Fact]
        public async Task DeleteArticlesFromDB()
        {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();
            // Insert seed data into the database using one instance of the context
            using (var context = new WebShopRepository(options))
            {       

                context.Articles.Add(new Articles
                {
                    ArticleId = 1,
                    ArticleNumber = "123456789",
                    ArticlePrice = 1234,
                    ArticleAddDate = DateTime.Now,
                    ArticleStock = 2,
                    ImageId = 1,
                    VendorId = 1000,
                    CategoryId = 9999,
                    ProductId = 2222,
                    SubCategoryId = 4444,
                    ISActive = true,
                    ISCampaign = false,
                    ArticleGuid = _guid,
                    _Category = null,
                    _Image = null,
                    _Product = null,
                    _SubCategory = null,
                    _Vendor = null
                });
                context.ArticleTranslations.Add(new ArticleTranslation
                {
                    ArticleId = 1,
                    ArticleName = "Test Product 1",
                    ArticleShortText = "ShortText",
                    ArticleFeaturesOne = "One",
                    ArticleFeaturesTwo = "Two",
                    ArticleFeaturesThree = "Three",
                    ArticleFeaturesFour = "Four",
                    LangCode = "sv",
                    ArticleNumber = "123456789",
                    ISTranslated = true,
                });
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WebShopRepository(options))
            {
                var service = new ArticleController(null,context, null, null);
                //Act
                var result = await service.DeleteConfirmed(1);
                //Assert
                Assert.Equal(0, context.Articles.Count());
            }
        }

        //[Fact]
        //public async Task EditArticlesIdDB()
        //{
        //    //Arrange
        //    // All contexts that share the same service provider will share the same InMemory database
        //    var options = CreateNewContextOptions();
        //    // Insert seed data into the database using one instance of the context
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Articles.Add(new Articles
        //        {
        //            ArticleId = 1,
        //            ArticleAddDate = new System.DateTime(2000, 1, 1),
        //            ArticleNumber = "9999000222002",
        //            ArticlePrice = 2999,
        //            ArticleStock = 3,
        //            ISActive = true,
        //            ISCampaign = false,
        //            CategoryId = 1001,
        //            ProductId = 111,
        //            SubCategoryId = 999,
        //            VendorId = 9111,

        //        });
        //        context.Articles.Add(new Articles
        //        {
        //            ArticleId = 1,
        //            ArticleAddDate = new System.DateTime(2000, 1, 1),
        //            ArticleNumber = "9999000222002",
        //            ArticlePrice = 2999,
        //            ArticleStock = 3,
        //            ISActive = true,
        //            ISCampaign = false,
        //            CategoryId = 1001,
        //            ProductId = 111,
        //            SubCategoryId = 999,
        //            VendorId = 9111,
        //        });
        //        context.SaveChanges();
        //    }

        //    // Use a clean instance of the context to run the test
        //    using (var context = new WebShopRepository(options))
        //    {
        //        var service = new ArticleController(null,context, null, null);
        //        //Act
        //        var result = await service.Edit(1,
        //            new Articles
        //            {

        //                ArticleId = 1,
        //                ArticlePrice = 2999,
        //                ArticleStock = 3,
        //                ISActive = true,
        //                ISCampaign = false,
        //                CategoryId = 1001,
        //                ProductId = 111,
        //                SubCategoryId = 999,
        //                VendorId = 9111,
        //                ImageId = 1,
        //                ArticleAddDate = DateTime.Now,


        //                //ArticleName = "Changed Name",
        //                //ArticleAddDate = new System.DateTime(2000, 1, 1),
        //                //ArticleFeaturesFour = "Blalalal",
        //                //ArticleFeaturesOne = "asdfasdf",
        //                //ArticleFeaturesThree = "asdfasdf",
        //                //ArticleFeaturesTwo = "asdfadsf",
        //                //ArticleGuid = "987638c9-50eb-42f2-81a6-a519439d168f",
        //                //ArticleImgPath = "",
        //                //ArticleNumber = "9999000222002",
        //                //ArticleShortText = "baflbkj abfb abfbsfb",
        //            });

        //        //Assert

        //        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        //        var dbCheck = context.Articles.SingleOrDefault(x => x.ArticleId == 1);

        //        Assert.Equal("Changed Name", dbCheck);
        //    }
        //}

        //[Fact]
        //public void SearchFunction()
        //{
        //    string search = "Sony";
        //    //Arrange
        //    // All contexts that share the same service provider will share the same InMemory database
        //    var options = CreateNewContextOptions();

        //    // Insert seed data into the database using one instance of the context
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Articles.Add(new Articles
        //        {
        //            ArticleID = 1,
        //            ArticleName = "Sony",
        //            ArticleAddDate = new System.DateTime(2000, 1, 1),
        //            ArticleFeaturesFour = "Blalalal",
        //            ArticleFeaturesOne = "asdfasdf",
        //            ArticleFeaturesThree = "asdfasdf",
        //            ArticleFeaturesTwo = "asdfadsf",
        //            ArticleGuid = new Guid("987638c9-50eb-42f2-81a6-a519439d168f"),
        //            ArticleImgPath = "",
        //            ArticleNumber = "9999000222002",
        //            ArticlePrice = 2999,
        //            ArticleShortText = "baflbkj abfb abfbsfb",
        //            ArticleStock = 3,
        //            ISActive = true,
        //            ISCampaign = false,
        //            CategoryID = 1001,
        //            ProductID = "0001",
        //            SubCategoryID = 999,
        //            VendorID = 9111,
        //            ProductImgPathID = 1
        //        });
        //        context.Articles.Add(new Articles
        //        {
        //            ArticleID = 2,
        //            ArticleName = "Adidas",
        //            ArticleAddDate = new System.DateTime(2000, 1, 1),
        //            ArticleFeaturesFour = "Blalalal",
        //            ArticleFeaturesOne = "asdfasdf",
        //            ArticleFeaturesThree = "asdfasdf",
        //            ArticleFeaturesTwo = "asdfadsf",
        //            ArticleGuid = "987638c9-50eb-42f2-81a6-a519439d168f",
        //            ArticleImgPath = "",
        //            ArticleNumber = "9999000222002",
        //            ArticlePrice = 2999,
        //            ArticleShortText = "baflbkj abfb abfbsfb",
        //            ArticleStock = 3,
        //            ISActive = true,
        //            ISCampaign = false,
        //            CategoryID = 1001,
        //            ProductID = "0001",
        //            SubCategoryID = 999,
        //            VendorID = 9111,
        //            ProductImgPathID = 1
        //        });
        //        context.SaveChanges();
        //    }

        //    // Use a clean instance of the context to run the test
        //    using (var context = new WebShopRepository(options))
        //    {
        //        var service = new ArticleController(context, hostEnvironment, localizer);
        //        //Act
        //        var query = service.Search(search);

        //        //Assert
        //        var viewResult = Assert.IsType<ViewResult>(query);
        //        var model = Assert.IsAssignableFrom<IEnumerable<Articles_old>>(
        //            viewResult.ViewData.Model);
        //        Assert.Equal(1, model.Count());
        //    }
        //}
    }
}