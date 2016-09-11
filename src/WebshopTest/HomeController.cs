using Microsoft.AspNetCore.Mvc;
using System;
using Webshop.Interfaces;
using Webshop.ViewModels;
using System.Linq;
using Xunit;
using Webshop.Models;

namespace Webshop.Controllers
{
    class StaticDateTime : IDateTime
    {
        public DateTime Now {
            get {
                return new DateTime(2016, 09, 01, 6, 0, 0);
            }
        }
    }
    public class HomeControllerTest
        
    {
        private WebShopRepository context;

        [Fact]
        public void HomeControllerContactTest()
        {
            //Arrange
            var datetime = new StaticDateTime();
            var controller = new HomeController(context);
            //Act
            var result = controller.Contact(datetime);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ContactViewModel>(viewResult.ViewData.Model);
            Assert.Equal(datetime.Now.ToString(), model.CurrentDateAndTime);
            Assert.Equal(2,model.Names.Count());
            Assert.Equal("Andreas", model.Names.ElementAt(0));
        }
    }
}
