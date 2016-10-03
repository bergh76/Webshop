using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Webshop.Models;
using System.Threading;
using Webshop.Models.BusinessLayers;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Controllers
{
    public class CheckOutController : Controller
    {
        private const string PromoCode = "FREE";

        private readonly ILogger<CheckOutController> _logger;
        private readonly WebShopRepository _context;
        public CheckOutController(ILogger<CheckOutController> logger, WebShopRepository context)
        {
            _context = context;
            _logger = logger;
        }

        //
        // GET: /Checkout/
        public IActionResult AddressAndPayment(string id)
        {
            var cart = _context.CartItems.Where(x => x.CartId == id).ToList();
            foreach (var item in cart)
            {
                var cartItems = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                        {
                            { "reference", item.ArticleId },
                            { "name", item.ArticleName },
                            { "quantity", item.Count },
                            { "unit_price", Convert.ToInt32(item.Article.ArticlePrice)*100 },
                            { "discount_rate", 00 },
                            { "tax_rate", 2500 }
                        },
                    //new Dictionary<string, object>
                    //    {
                    //        { "type", "shipping_fee" },
                    //        { "reference", "SHIPPING" },
                    //        { "name", "Shipping Fee" },
                    //        { "quantity", 1 },
                    //        { "unit_price", 4900 },
                    //        { "tax_rate", 2500 }
                    //    }
                };
                var c = new Dictionary<string, object> { { "items", cartItems } };
            }


            var data = new Dictionary<string, object>
            {
                { "cart", cart }
            };
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddressAndPayment([FromServices] WebShopRepository dbContext,[FromForm] Order order, CancellationToken requestAborted)
        {
            
            if (!ModelState.IsValid)
            {
                return View(order);
            }
           
            //var formCollection = await HttpContext.Request.ReadFormAsync();

            //try
            //{
            //    if (string.Equals(formCollection["PromoCode"].FirstOrDefault(), PromoCode,
            //        StringComparison.OrdinalIgnoreCase) == false)
            //    {
            //        return View(order);
            //    }
            //    else
            //    {
            //        order.Username = HttpContext.User.Identity.Name;
            //        order.OrderDate = DateTime.Now;

            //        //Add the Order
            //        dbContext.Orders.Add(order);

            //        //Process the order
            //        var cart = ShoppingCart.GetCart(dbContext, HttpContext);
            //        await cart.CreateOrder(order);

            //        _logger.LogInformation("User {userName} started checkout of {orderId}.", order.Username, order.OrderId);

            //        // Save all changes
            //        await dbContext.SaveChangesAsync(requestAborted);

            //        return RedirectToAction("Complete", new { id = order.OrderId });
            //    }
            //}
            //catch
            //{
            //    //Invalid - redisplay with errors
            return View(order);
        }


        //
        // GET: /Checkout/Complete

        public async Task<IActionResult> Complete(
            [FromServices] WebShopRepository dbContext,
            int id)
        {
            var userName = HttpContext.User.Identity.Name;

            // Validate customer owns this order
            bool isValid = await dbContext.Orders.AnyAsync(
                o => o.OrderId == id &&
                o.Username == userName);

            if (isValid)
            {
                _logger.LogInformation("User {userName} completed checkout on order {orderId}.", userName, id);
                return View(id);
            }
            else
            {
                _logger.LogError(
                    "User {userName} tried to checkout with an order ({orderId}) that doesn't belong to them.",
                    userName,
                    id);
                return View("Error");
            }
        }
    }
}