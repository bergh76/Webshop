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
            var userId = dbContext.Users.Where(x => x.UserName == Convert.ToString(user)).Select(x => x.Id).SingleOrDefault();

            if (userId == dbContext.Orders
                .Distinct()
                .Where(x => x.Username == Convert.ToString(user))
                .Select(x => x.UserId)
                .SingleOrDefault())
            {
                var artList = from o in dbContext.Orders
                              join od in dbContext.OrderDetails on o.OrderId equals od.OrderId
                              join u in dbContext.Users on o.UserId equals u.Id
                              //join a in dbContext.ArticleTranslations on od.ArticleId equals a.ArticleId
                              
                              select new OrderOverviewViewModel
                              {
                                  OrderId = o.OrderId,
                                  ArticleId = od.ArticleId,
                                  ArticleNumber = od.ArticleNumber,
                                  ArticleName = dbContext.ArticleTranslations
                                                                     .Where(x => x.ArticleId == od.ArticleId)
                                                                     .Select(x => x.ArticleName)
                                                                     .FirstOrDefault(),
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