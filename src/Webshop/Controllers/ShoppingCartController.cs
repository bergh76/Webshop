using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.ViewModels;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Webshop.Services;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
namespace Webshop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private static string _iso;
        private static decimal _curr;
        public ShoppingCartController([FromServices]FixerIO fixer,WebShopRepository context, ILogger<ShoppingCartController> logger)
        {
            _iso = new RegionInfo(CultureInfo.CurrentUICulture.Name).ISOCurrencySymbol;
            _curr = FixerIO.GetUDSToRate(_iso);
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> SearchArticles( int vendor, int category, int product, int subproduct)
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            var artList = from p in _context.Articles
                          where p.VendorId == vendor || vendor == 0
                          where p.CategoryId == category || category == 0
                          where p.ProductId == product || product == 0
                          where p.SubCategoryId == subproduct || subproduct == 0
                          join i in _context.Images on p.ArticleId equals i.ArtikelId
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }
                          select new ArticlesViewModel
                          {
                              ArticleId = p.ArticleId,
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice /_curr,
                              ArticleStock = p.ArticleStock,
                              ISActive = p.ISActive,
                              ISCampaign = p.ISCampaign,
                              ArticleName = pt.ArticleName,
                              ArticleShortText = pt.ArticleShortText,
                              ArticleFeaturesOne = pt.ArticleFeaturesOne,
                              ArticleFeaturesTwo = pt.ArticleFeaturesTwo,
                              ArticleFeaturesThree = pt.ArticleFeaturesThree,
                              ArticleFeaturesFour = pt.ArticleFeaturesFour,
                              ArticleImgPath = i.ImagePath + i.ImageName,
                          };

            IEnumerable<ArticlesViewModel> vModel = await artList.ToListAsync();

            return View(vModel.ToList());
        }
        public WebShopRepository _context { get; }

        //
        // GET: /ShoppingCart/
        public async Task<IActionResult> Index()
        {
            var cart = ShoppingCart.GetCart(_context, HttpContext); // gets all items from context.CartItems

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = await cart.GetCartItems()   ,
                CartTotal = await cart.GetTotal()
            };

            // Return the view
            return View("ShoppingCart", viewModel);
        }

        //
        // GET: /ShoppingCart/AddToCart/5

        public async Task<IActionResult> AddToCart(int id, CancellationToken requestAborted)
        {
            // Retrieve the album from the database
            var addedArticle = await _context.Articles
                .SingleAsync(article => article.ArticleId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(_context, HttpContext);

            await cart.AddToCart(addedArticle);//, result);

            await _context.SaveChangesAsync(requestAborted);
            _logger.LogInformation("Article {0} was added to the cart.", addedArticle.ArticleId);

            // Go back to the main store page for more shopping
            return RedirectToAction("SearchArticles");
        }

        // GET: /Checkout/
        //public ViewResult AddressAndPayment()//string id, ShoppingCartViewModel items)
        //{
        //    //    foreach (var item in items.CartItems.Where(x => x.CartId == id))
        //    //    {
        //    var cartItems = new List<Dictionary<string, object>>
        //    {
        //        new Dictionary<string, object>
        //        {
        //            { "reference", "123456" }, //item.Article.ArticleNumber },
        //            { "name", "Strumpor" },//item.ArticleName },
        //            { "quantity", 2 },//item.Count },
        //            { "unit_price", 9900 },// Convert.ToInt32(item.Article.ArticlePrice)*100 },
        //            { "discount_rate", 1000 },
        //            { "tax_rate", 2500 }
        //        },
        //        new Dictionary<string, object>
        //        {
        //            { "type", "shipping_fee" },
        //            { "reference", "SHIPPING" },
        //            { "name", "Shipping Fee" },
        //            { "quantity", 1 },
        //            { "unit_price", 4900 },
        //            { "tax_rate", 2500 }
        //        }
        //    };
        //    var cart = new Dictionary<string, object> { { "items", cartItems } };

        //    var data = new Dictionary<string, object>
        //    {
        //        { "cart", cart }
        //    };
        //    var merchant = new Dictionary<string, object>
        //    {
        //        { "id", "5160" },
        //        { "back_to_store_uri", "http://example.com" },
        //        { "terms_uri", "http://example.com/terms.aspx" },
        //        {
        //            "checkout_uri",
        //            "https://example.com/checkout.aspx"
        //        },
        //        {
        //            "confirmation_uri",
        //            "https://example.com/thankyou.aspx" +
        //            "?klarna_order_id={checkout.order.id}"
        //        },
        //        {
        //            "push_uri",
        //            "https://example.com/push.aspx" +
        //            "?klarna_order_id={checkout.order.id}"
        //        }
        //    };
        //    data.Add("purchase_country", "SE");
        //    data.Add("purchase_currency", "SEK");
        //    data.Add("locale", "sv-se");
        //    data.Add("merchant", merchant);
        //    Klarna k = new Klarna();
        //    var gui = k.CreateOrder(JsonConvert.SerializeObject(data));

        //    return View(gui);
        //}
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(
            int id,
            CancellationToken requestAborted)
        {
            // Retrieve the current user's shopping cart
            var cart = ShoppingCart.GetCart(_context, HttpContext);

            // Get the name of the album to display confirmation
            var cartItem = await _context.CartItems
                .Where(item => item.CartItemId == id)
                .Include(x => x.Article)
                .SingleOrDefaultAsync();

            string message;
            int itemCount;
            if (cartItem != null)
            {
                // Remove from cart
                itemCount = cart.RemoveFromCart(id);
                await _context.SaveChangesAsync(requestAborted);
                string removed = (itemCount > 0) ? " 1 copy of " : string.Empty;
                message = removed + cartItem.ArticleName + " has been removed from your shopping cart.";
            }
            else
            {
                itemCount = 0;
                message = "Could not find this item, nothing has been removed from your shopping cart.";
            }

            // Display the confirmation message

            var results = new ShoppingCartRemoveViewModel
            {
                Message = message,
                CartTotal = await cart.GetTotal(),
                CartCount = await cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            _logger.LogInformation("Album {id} was removed from a cart.", id);
            ViewData["Message"] = message;
            return Json(results);
        }
    }
}