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
using Webshop.BusinessLayers;
using Microsoft.Extensions.Localization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Webshop.Controllers
{
    //public class ArticlesControllerTest

    //{
    //    private readonly IHostingEnvironment hostEnvironment;
    //    private readonly IStringLocalizer<ArticleController> localizer;
    //    private static DbContextOptions<WebShopRepository> CreateNewContextOptions()
    //    {
    //        // Create a fresh service provider, and therefore a fresh 
    //        // InMemory database instance.
    //        var serviceProvider = new ServiceCollection()
    //            .AddEntityFrameworkInMemoryDatabase()
    //            .BuildServiceProvider();

    //        // Create a new options instance telling the context to use an
    //        // InMemory database and the new service provider.
    //        var builder = new DbContextOptionsBuilder<WebShopRepository>();
    //        builder.UseInMemoryDatabase()
    //               .UseInternalServiceProvider(serviceProvider);

    //        return builder.Options;
    //    }

    //    [Fact]
    //    public async Task IndexListAllArticles()
    //    {
    //        //Arrange
    //        // All contexts that share the same service provider will share the same InMemory database
    //        var options = CreateNewContextOptions();
    //        string dropdownVendor = "Sony";
    //        string dropdownCategory = "Ljud & Bild";
    //        string dropdownProduct = "Hemmabio & HiFi";
    //        string dropdownSubCategory = "7.2 Reciever";

    //        int vendorID = 9001;
    //        string categoryID = "1001";
    //        int productID = 0001;
    //        int subProductID = 0001;

    //        // Insert seed data into the database using one instance of the context            
    //        using (var context = new WebShopRepository(options))
    //        {
    //            context.Vendors.Add(new VendorModel { VendorID = 9001, VendorName = "Sony" });
    //            context.Vendors.Add(new VendorModel { VendorID = 9002, VendorName = "Rotel" });
    //            context.SaveChanges();
    //        }
    //        using (var context = new WebShopRepository(options))
    //        {
    //            context.Categories.Add(new CategoryModel { CategoryID = 1001, CategoryName = "Ljud & Bild" });
    //            context.Categories.Add(new CategoryModel { CategoryID = 1002, CategoryName = "Ljud & Bild" });
    //            context.SaveChanges();
    //        }
    //        using (var context = new WebShopRepository(options))
    //        {
    //            context.Products.Add(new ProductModel { ProductID = "1001", ProductName = "Hemmabio & HiFi" });
    //            context.Products.Add(new ProductModel { ProductID = "1001", ProductName = "Hemmabio & HiFi" });
    //            context.SaveChanges();
    //        }
    //        using (var context = new WebShopRepository(options))
    //        {
    //            context.SubCategories.Add(new SubCategoryModel { SubCategoryID = 0001, SubCategoryName = "7.2 Reciever" });
    //            context.SubCategories.Add(new SubCategoryModel { SubCategoryID = 0002, SubCategoryName = "Förstärkare" });
    //            context.SaveChanges();
    //        }
    //        using (var context = new WebShopRepository(options))
    //        {
    //            context.Articles.Add(new Articles { ArticleName = "Test Product 1", VendorID = 9001, CategoryID = 1001, ProductID = "1001", SubCategoryID = 0001 });
    //            context.Articles.Add(new Articles { ArticleName = "Test Product 2", VendorID = 9002, CategoryID = 1002, ProductID = "1001", SubCategoryID = 0002 });
    //            context.SaveChanges();
    //        }

    //        // Use a clean instance of the context to run the test
    //        using (var context = new WebShopRepository(options))
    //        {
    //            var service = new ArticleController(context, hostEnvironment, localizer);
    //            //Act
    //            var result = await service.Index(dropdownCategory, dropdownProduct, dropdownSubCategory, dropdownVendor, vendorID, productID, categoryID, subProductID);
    //            //Assert
    //            var viewResult = Assert.IsType<ViewResult>(result);
    //            var model = Assert.IsAssignableFrom<IEnumerable<Articles_old>>(
    //                viewResult.ViewData.Model);
    //            Assert.Equal(2, model.Count());
    //            Assert.Equal("Test Product 1", model.ElementAt(0).ArticleName);
    //        }
    //    }
    //    [Fact]
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

        //[Fact]
        //public async Task DeleteArticlesFromDB()
        //{
        //    //Arrange
        //    // All contexts that share the same service provider will share the same InMemory database
        //    var options = CreateNewContextOptions();

        //    // Insert seed data into the database using one instance of the context
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Articles.Add(new Articles { ArticleName = "Test Product 1" });
        //        context.Articles.Add(new Articles { ArticleName = "Test Product 2" });
        //        context.SaveChanges();
        //    }

        //    // Use a clean instance of the context to run the test
        //    using (var context = new WebShopRepository(options))
        //    {
        //        var service = new ArticleController(context, hostEnvironment, localizer);
        //        //Act
        //        var result = await service.DeleteConfirmed(1);
        //        //Assert
        //        Assert.Equal(1, context.Articles.Count());
        //    }
        //}

        //[Fact]
        //public async Task EditArticlesIdDB()
        //{
        //    //Arrange
        //    // All contexts that share the same service provider will share the same InMemory database
        //    var options = CreateNewContextOptions();
        //    // Insert seed data into the database using one instance of the context
        //    using (var context = new WebShopRepository(options))
        //    {
        //        context.Articles.Add(new ArticleModel
        //        {
        //            ArticleID = 1,
        //            ArticleName = "ProductName 1",
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
        //            ProductID = 111,
        //            SubCategoryID = 999,
        //            VendorID = 9111,
        //            ProductImgPathID = 1
        //        });
        //        context.Articles.Add(new ArticleModel
        //        {
        //            ArticleID = 2,
        //            ArticleName = "ProductName 2",
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
        //            ProductID = 111,
        //            SubCategoryID = 999,
        //            VendorID = 9111,
        //            ProductImgPathID = 1
        //        });
        //        context.SaveChanges();
        //    }

        //    // Use a clean instance of the context to run the test
        //    using (var context = new WebShopRepository(options))
        //    {
        //        var service = new ArticleController(context, hostEnvironment);
        //        //Act
        //        var result = await service.Edit(1,
        //            new ArticleModel
        //            {
        //                ArticleID = 1,
        //                ArticleName = "Changed Name",
        //                ArticleAddDate = new System.DateTime(2000, 1, 1),
        //                ArticleFeaturesFour = "Blalalal",
        //                ArticleFeaturesOne = "asdfasdf",
        //                ArticleFeaturesThree = "asdfasdf",
        //                ArticleFeaturesTwo = "asdfadsf",
        //                ArticleGuid = "987638c9-50eb-42f2-81a6-a519439d168f",
        //                ArticleImgPath = "",
        //                ArticleNumber = "9999000222002",
        //                ArticlePrice = 2999,
        //                ArticleShortText = "baflbkj abfb abfbsfb",
        //                ArticleStock = 3,
        //                ISActive = true,
        //                ISCampaign = false,
        //                CategoryID = 1001,
        //                ProductID = 111,
        //                SubCategoryID = 999,
        //                VendorID = 9111,
        //                ProductImgPathID = 1
        //            });

        //        //Assert

        //        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        //        var dbCheck = context.Articles.SingleOrDefault(x => x.ArticleID == 1).ArticleName;

        //        Assert.Equal("Changed Name", dbCheck );
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
        //        var query =  service.Search(search);

        //        //Assert
        //        var viewResult = Assert.IsType<ViewResult>(query);
        //        var model = Assert.IsAssignableFrom<IEnumerable<Articles_old>>(
        //            viewResult.ViewData.Model);
        //        Assert.Equal(1, model.Count());
        //    }
        //}
    //}
}