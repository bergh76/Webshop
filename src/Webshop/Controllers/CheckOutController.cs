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
using Microsoft.AspNetCore.Identity;

namespace Webshop.Controllers
{
    public class CheckOutController : Controller
    {
        private const string PromoCode = "FREE";
        private readonly ILogger<CheckOutController> _logger;
        private readonly WebShopRepository _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckOutController(
             UserManager<ApplicationUser> userManager,
            ILogger<CheckOutController> logger, 
            WebShopRepository context
            )
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        //
        // GET: /Checkout/
        public async Task<IActionResult> Checkout(
            [FromServices] WebShopRepository dbContext,
            [FromForm] Order order,
            OrderDetail orderDetials,
            CancellationToken requestAborted
            )
        {

            //Process the order
            var getCart = ShoppingCart.GetCart(dbContext, HttpContext);  


            _logger.LogInformation("User {userName} started checkout of {orderId}.", order.Username, order.OrderId);
            //if (order.OrderDetails != null)
            if (getCart.GetCartItems() != null)
            {
                try
                {
                    var cartItems = new List<Dictionary<string, object>>();
                    foreach (var item in await getCart.GetCartItems())
                    {
                        cartItems.Add(new Dictionary<string, object> {
                            { "reference", item.ArticleNumber },
                            { "name", item.ArticleName },
                            { "quantity", item.Count },
                            { "unit_price", (int)(item.Article.ArticlePrice)*100 },
                            { "discount_rate", 000 },
                            { "tax_rate", 2500 }
                        }
                        );
                    }
                    new Dictionary<string, object>
                        {
                            { "type", "shipping_fee" },
                            { "reference", "SHIPPING" },
                            { "name", "Shipping Fee" },
                            { "quantity", 1 },
                            { "unit_price", 4900 },
                            { "tax_rate", 2500 }
                        };


                    var cart = new Dictionary<string, object> { { "items", cartItems } };
                    var data = new Dictionary<string, object>
                    {
                        { "cart", cart }
                    };
                    var merchant = new Dictionary<string, object>
                    {
                        { "id", "5160" },
                        { "back_to_store_uri", "http://localhost:5000/" },
                        { "terms_uri", "http://example.com/terms.aspx" },
                        {
                            "checkout_uri",
                            "http://localhost:5000/CheckOut/Checkout/"
                        },
                        {
                            "confirmation_uri",
                            "http://localhost:5000/CheckOut/Complete/" +
                            "?klarna_order_id={checkout.order.id}"
                        },
                        {
                            "push_uri",
                            "http://localhost:5000/CheckOut/Complete/" +
                            "?klarna_order_id={checkout.order.id}"
                        }
                    };
                    //
                    // have not yet obtained your merchant ID, please apply for API credentials
                    // Testing environment: https://checkout.testdrive.klarna.com
                    // Merchant ID (eid)and shared secret: Please use the Merchant ID(eid) and shared secret provided to you by Klarna.
                    // E-mail address: checkout-se@testdrive.klarna.com
                    // Postal code: 12345
                    // Personal identity number: 410321-9202
                    
                                        data.Add("purchase_country", "SE");
                    data.Add("purchase_currency", "SEK");
                    data.Add("locale", "sv-se");
                    data.Add("merchant", merchant);
                    var k = new Klarna();
                    var gui = k.CreateOrder(JsonConvert.SerializeObject(data));

                    order.Username = HttpContext.User.Identity.Name;
                    order.UserId = dbContext.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
                                .Select(x => x.Id)
                                .FirstOrDefault();
                    order.OrderDate = DateTime.Now;

                    //Add the Order
                    dbContext.Orders.Add(order);
                    dbContext.SaveChanges();
                    await getCart.CreateOrder(order);

                    return View("Checkout", gui);
                }
                catch (WebException ex)
                {
                    var webException = ex.InnerException as WebException;
                    if (webException != null)
                    {
                        // Here you can check for timeouts, and other connection related errors.
                        // webException.Response could contain the response object.
                        throw new WebException(webException.Message);
                    }
                    else
                    {
                        // In case there wasn't a WebException where you could get the response
                        var faultResponse = (string)ex.Data["Respone"];
                        throw new Exception(faultResponse);
                    }
                }
            }
            else
            {
                _logger.LogInformation("There was a problem with Order:{orderId}.", order.OrderId);

                return PartialView("_ErrorCheckOut");
            }
           
        }

        public ViewResult Complete([FromForm] Order order, [FromServices] WebShopRepository dbContext, string klarna_order_id)
        {
           
            Klarna k = new Klarna();
            var gui = k.KlarnaConfirmation(klarna_order_id);

            return View("Complete", gui);
        }
       
    }
}        