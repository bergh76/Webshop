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
using Webshop.Services;
using Newtonsoft.Json;
using System.Net;

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
        public ViewResult AddressAndPayment()//string id, ShoppingCartViewModel items)
        {
            //    foreach (var item in items.CartItems.Where(x => x.CartId == id))
            //    {
            try
            {
                var cartItems = new List<Dictionary<string, object>>
            {

                new Dictionary<string, object>
                {
                    { "reference", "123456" }, //item.Article.ArticleNumber },
                    { "name", "Strumpor" },//item.ArticleName },
                    { "quantity", 2 },//item.Count },
                    { "unit_price", 9900 },// Convert.ToInt32(item.Article.ArticlePrice)*100 },
                    { "discount_rate", 1000 },
                    { "tax_rate", 2500 }
                },
                new Dictionary<string, object>
                {
                    { "type", "shipping_fee" },
                    { "reference", "SHIPPING" },
                    { "name", "Shipping Fee" },
                    { "quantity", 1 },
                    { "unit_price", 4900 },
                    { "tax_rate", 2500 }
                }
            };
                var cart = new Dictionary<string, object> { { "items", cartItems } };

                var data = new Dictionary<string, object>
            {
                { "cart", cart }
            };
                var merchant = new Dictionary<string, object>
            {
                { "id", "5160" },
                { "back_to_store_uri", "http://example.com" },
                { "terms_uri", "http://example.com/terms.aspx" },
                {
                    "checkout_uri",
                    "https://example.com/checkout.aspx"
                },
                {
                    "confirmation_uri",
                    "https://example.com/thankyou.aspx" +
                    "?klarna_order_id={checkout.order.id}"
                },
                {
                    "push_uri",
                    "https://example.com/push.aspx" +
                    "?klarna_order_id={checkout.order.id}"
                }
            };
                data.Add("purchase_country", "SE");
                data.Add("purchase_currency", "SEK");
                data.Add("locale", "sv-se");
                data.Add("merchant", merchant);
                Klarna k = new Klarna();
                var gui = k.CreateOrder(JsonConvert.SerializeObject(data));

                return View(gui);
            }
            catch (WebException ex)
            {
                var webException = ex.InnerException as WebException;
                if (webException != null)
                {
                    // Here you can check for timeouts, and other connection related errors.
                    // webException.Response could contain the response object.
                }
                else
                {
                    // In case there wasn't a WebException where you could get the response
                    // (e.g. a protocol error, bad digest, etc) you might still be able to
                    // get a hold of the response object.
                    // ex.Data["Response"] as IHttpResponse
                }

                // Additional data might be available in ex.Data.
                if (ex.Data.Contains("internal_message"))
                {
                    // For instance, Content-Type application/vnd.klarna.error-v1+json has "internal_message".
                    var internalMessage = (string)ex.Data["internal_message"];
                    throw new WebException(internalMessage);
                }

                throw;
            }
            catch (Exception)
            {
                // Something else went wrong, e.g. invalid arguments passed to the order object.
                throw;
            }
        }
    

        //
        // POST: /Checkout/AddressAndPayment

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddressAndPayment([FromServices] WebShopRepository dbContext,[FromForm] Order order, CancellationToken requestAborted)
        //{
            
        //    if (!ModelState.IsValid)
        //    {
        //        return View(order);
        //    }
           
        //    //var formCollection = await HttpContext.Request.ReadFormAsync();

        //    //try
        //    //{
        //    //    if (string.Equals(formCollection["PromoCode"].FirstOrDefault(), PromoCode,
        //    //        StringComparison.OrdinalIgnoreCase) == false)
        //    //    {
        //    //        return View(order);
        //    //    }
        //    //    else
        //    //    {
        //    //        order.Username = HttpContext.User.Identity.Name;
        //    //        order.OrderDate = DateTime.Now;

        //    //        //Add the Order
        //    //        dbContext.Orders.Add(order);

        //    //        //Process the order
        //    //        var cart = ShoppingCart.GetCart(dbContext, HttpContext);
        //    //        await cart.CreateOrder(order);

        //    //        _logger.LogInformation("User {userName} started checkout of {orderId}.", order.Username, order.OrderId);

        //    //        // Save all changes
        //    //        await dbContext.SaveChangesAsync(requestAborted);

        //    //        return RedirectToAction("Complete", new { id = order.OrderId });
        //    //    }
        //    //}
        //    //catch
        //    //{
        //    //    //Invalid - redisplay with errors
        //    return View(order);
        //}


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