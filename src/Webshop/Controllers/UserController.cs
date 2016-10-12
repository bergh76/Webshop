using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webshop.Models;
using System.Globalization;
using Webshop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Webshop.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(
                   UserManager<ApplicationUser> userManager,
                   SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Orders([FromServices] WebShopRepository dbContext)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = dbContext.Users.Where(x => x.UserName == Convert.ToString(user)).Select(x => x.Id).FirstOrDefault();

            if (userId == dbContext.Orders
                .Where(x => x.Username == Convert.ToString(user))
                .Select(x => x.UserId)
                .FirstOrDefault())
            {
                var artList = from o in dbContext.Orders
                              join u in dbContext.Users on o.UserId equals u.Id
                              join od in dbContext.OrderDetails on o.OrderId equals od.OrderId
                              join a in dbContext.Articles on od.ArticleId equals a.ArticleId
                              join at in dbContext.ArticleTranslations on
                                                            new { a.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                                            equals new { at.ArticleId, Second = at.LangCode }
                              where u.Id == o.UserId
                              select new OrderOverviewViewModel
                              {
                                  OrderId = o.OrderId,
                                  ArticleId = od.ArticleId,
                                  ArticleNumber = od.ArticleNumber,
                                  ArticleName = at.ArticleName,
                                  Quantity = od.Quantity,
                                  UnitPrice = od.UnitPrice,
                                  Total = o.Total,
                                  OrderDate = o.OrderDate,
                                  KlarnaOrderId = o.KlarnaOrderId,
                                  UserId = o.UserId
                              };
                
                

                IEnumerable<OrderOverviewViewModel> vModel = artList.ToList();
                return View(vModel);
            }
            return View();
        }
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}