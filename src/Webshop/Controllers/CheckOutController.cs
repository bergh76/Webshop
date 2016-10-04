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
        public async Task<IActionResult> Checkout(
            [FromServices] WebShopRepository dbContext,
            [FromForm] Order order,
            OrderDetail orderDetials,
            CancellationToken requestAborted)//string id, ShoppingCartViewModel items)
        {
            order.Username = HttpContext.User.Identity.Name;
            order.OrderDate = DateTime.Now;

            //Add the Order
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            //Process the order
            var getCart = ShoppingCart.GetCart(dbContext, HttpContext);
            await getCart.CreateOrder(order);
            //var cId = order.OrderId;
            
            _logger.LogInformation("User {userName} started checkout of {orderId}.", order.Username, order.OrderId);
            if (order.OrderDetails != null)
            {
                try
                {
                    
                    var cartItems = new List<Dictionary<string, object>>
                    {                      
                    
                        new Dictionary<string, object>
                        {                                
                            { "reference", "111111111" }, //item.ArticleId },
                            { "name", "dfgsdf" }, //item.ArticleName },
                            { "quantity", 1 }, //item.Quantity },
                            { "unit_price", 12500 }, //Convert.ToInt32(item.UnitPrice)*100 },
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
                        { "back_to_store_uri", "http://localhost:5000/" },
                        { "terms_uri", "http://example.com/terms.aspx" },
                        {
                            "checkout_uri",
                            "http://localhost:5000/CheckOut/AddressAndPayment/"
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
                        throw new WebException(webException.Message);
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
            }
            else
            {
                _logger.LogInformation("There was a problem wiht Order:{orderId}.", order.OrderId);

                return PartialView("_ErrorCheckOut");
            }
        }
    }
}

           