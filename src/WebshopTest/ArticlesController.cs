// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
        //private readonly ILogger<ArticleController> _logger;
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

        //public void CreateSeedData()
        //{
        //    var options = CreateNewContextOptions();

        //    int vendorID = 9001;
        //    int categoryID = 1001;
        //    int productID = 1010;
        //    int subProductID = 2010;
        //    string lang = "sv";

        //    // Insert seed data into the database using one instance of the context            
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Vendors.Add(new VendorModel { ID = 1, VendorID = vendorID, VendorName = "Sony", LangCode = lang, ISActive = true, VendorWebPage = "http://www.asdf.com" });
        //        context.SaveChanges();
        //    }
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Categories.Add(new CategoryModel { ID = 1, CategoryID = categoryID, CategoryName = "Ljud & Bild", LangCode = lang, ISActive = true });
        //        context.SaveChanges();
        //    }
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Products.Add(new ProductModel { ID = 1, ProductID = productID, ProductName = "Hemmabio & HiFi", LangCode = lang, ISActive = true });
        //        context.SaveChanges();
        //    }
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.SubCategories.Add(new SubCategoryModel { ID = 1, SubCategoryID = subProductID, SubCategoryName = "7.2 Reciever", LangCode = lang, ISActive = true });
        //        context.SaveChanges();
        //    }
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Images.Add(new ImageModel { ImageDate = DateTime.Now, ImageName = "TestImage.jpg", ImagePath = "/images/products/", ArtikelId = 1 });
        //        context.SaveChanges();
        //        Articles article = new Articles()
        //        {
        //            ArticleId = 1,
        //            ArticleNumber = "123456789",
        //            ArticlePrice = 1234,
        //            ArticleAddDate = DateTime.Now,
        //            ArticleStock = 2,
        //            VendorId = vendorID,
        //            CategoryId = categoryID,
        //            ProductId = productID,
        //            SubCategoryId = subProductID,
        //            ISActive = true,
        //            ISCampaign = false,
        //            _Category = null,
        //            _Product = null,
        //            _SubCategory = null,
        //            _Vendor = null

        //        };
        //        context.Add(article);
        //        context.SaveChanges();
        //        ArticleTranslation artTrans = new ArticleTranslation()
        //        {
        //            ArticleId = 1,
        //            ArticleName = "Test Product 1",
        //            ArticleShortText = "ShortText",
        //            ArticleFeaturesOne = "One",
        //            ArticleFeaturesTwo = "Two",
        //            ArticleFeaturesThree = "Three",
        //            ArticleFeaturesFour = "Four",
        //            LangCode = lang,
        //            ISTranslated = true

        //        };
        //        context.Add(artTrans);
        //        context.SaveChanges();
        //    }

        //}

        [Fact] //OK
        public async Task IndexListAllArticles()
        {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();
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
                    ArticleName = "Test Product 1",
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
                var service = new ArticleController(null, context, null, hostEnvironment, localizer, null);
                //Act
                var result = await service.Index();
                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<ArticlesViewModel>>(
                    viewResult.ViewData.Model);
                Assert.Equal(1, model.Count());
                Assert.Equal("Test Product 1", model.ElementAt(0).ArticleName);
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

        [Fact] //OK
        public async Task DeleteArticlesFromDB()
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
                    ArticleName = "Test Product 1",
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
                var service = new ArticleController(null, context, null, null, null,null);
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
        //    int vendorID = 9001;
        //    int categoryID = 1001;
        //    int productID = 1010;
        //    int subProductID = 2010;
        //    string lang = "sv";

        //    // Insert seed data into the database using one instance of the context            
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Vendors.Add(new VendorModel { ID = 1, VendorID = vendorID, VendorName = "Sony", LangCode = lang, ISActive = true, VendorWebPage = "http://www.asdf.com" });
        //        context.SaveChanges();
        //    }
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Categories.Add(new CategoryModel { ID = 1, CategoryID = categoryID, CategoryName = "Ljud & Bild", LangCode = lang, ISActive = true });
        //        context.SaveChanges();
        //    }
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Products.Add(new ProductModel { ID = 1, ProductID = productID, ProductName = "Hemmabio & HiFi", LangCode = lang, ISActive = true });
        //        context.SaveChanges();
        //    }
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.SubCategories.Add(new SubCategoryModel { ID = 1, SubCategoryID = subProductID, SubCategoryName = "7.2 Reciever", LangCode = lang, ISActive = true });
        //        context.SaveChanges();
        //    }
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Images.Add(new ImageModel { ImageDate = DateTime.Now, ImageName = "TestImage.jpg", ImagePath = "/images/products/", ArtikelId = 1 });
        //        context.SaveChanges();
        //        Articles article = new Articles()
        //        {
        //            ArticleId = 1,
        //            ArticleNumber = "123456789",
        //            ArticlePrice = 1234,
        //            ArticleAddDate = DateTime.Now,
        //            ArticleStock = 2,
        //            ImageId = 1,
        //            VendorId = vendorID,
        //            CategoryId = categoryID,
        //            ProductId = productID,
        //            SubCategoryId = subProductID,
        //            ISActive = true,
        //            ISCampaign = false,
        //            _Category = null,
        //            _Product = null,
        //            _SubCategory = null,
        //            _Vendor = null

        //        };
        //        context.Add(article);
        //        context.SaveChanges();
        //        ArticleTranslation artTrans = new ArticleTranslation()
        //        {
        //            ArticleId = 1,
        //            ArticleName = "Test Product 1",
        //            ArticleShortText = "ShortText",
        //            ArticleFeaturesOne = "One",
        //            ArticleFeaturesTwo = "Two",
        //            ArticleFeaturesThree = "Three",
        //            ArticleFeaturesFour = "Four",
        //            LangCode = lang,
        //            ISTranslated = true

        //        };
        //        context.Add(artTrans);
        //        context.SaveChanges();
        //    }

        //    // Use a clean instance of the context to run the test
        //    using (var context = new WebShopRepository(options))
        //    {
        //        var service = new ArticleController(null, context, null, null,null,null);
        //        //Act
        //        var result = await service.Edit(1,
        //            new ArticleTranslation
        //            {
        //                ArticleName = "Edited Product 1",
        //                ArticleShortText = "ShortText",
        //                ArticleFeaturesOne = "One",
        //                ArticleFeaturesTwo = "Two",
        //                ArticleFeaturesThree = "Three",
        //                ArticleFeaturesFour = "Four",
        //                LangCode = lang,
        //                ISTranslated = true
        //            }
        //           );

        //        //Assert

        //        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        //        var dbCheck = context.ArticleTranslations.SingleOrDefault(x => x.ArticleId == 1);

        //        Assert.Equal("Edited Product 1", dbCheck.ArticleName);
        //    }
        //}


    }
}