using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webshop.Models;
using System.Globalization;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Orders([FromServices] WebShopRepository dbContext)
        {
            var artList = from o in dbContext.Orders
                          join od in dbContext.OrderDetails on o.OrderId equals od.OrderId
                                           //new { o.OrderId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           //equals new { od.ArticleId, Second = od.LangCode }

                          select new OrderOverviewViewModel
                          {
                             OrderId = o.OrderId,
                             ArticleNumber = od.ArticleNumber,
                             ArticleName = od.ArticleName,
                             Quantity = od.Quantity,
                             UnitPrice = od.UnitPrice,
                             Total = o.Total,
                             OrderDate = o.OrderDate,
                             KlarnaOrderId = od.KlarnaOrderId
                          };

            IEnumerable<OrderOverviewViewModel> vModel = artList.ToList();
            return View(vModel);
        }
    }
}