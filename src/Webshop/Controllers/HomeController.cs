

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Interfaces;

using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        private WebShopRepository _context;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly AppSettings _appSettings;

        public HomeController(WebShopRepository context, IStringLocalizer<HomeController> localizer, ILogger<ShoppingCartController> logger)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
            //_appSettings = options.Value;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult _SearchArticles(string vendor, string category, string product, string subproduct)
        {


            var artList = from p in _context.Articles
                              //where p.ISCampaign == true
                          join i in _context.Images on p.ArticleGuid equals i.ArticleGuid
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }
                          where p._Vendor.VendorName == vendor || string.IsNullOrEmpty(vendor)
                          where p._Category.CategoryName == category || string.IsNullOrEmpty(category)
                          where p._Product.ProductName == product || string.IsNullOrEmpty(product)
                          where p._SubCategory.SubCategoryName == subproduct || string.IsNullOrEmpty(subproduct)

                          select new ArticlesViewModel
                          {
                              ArticleId = p.ArticleId,
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice,
                              ArticleStock = p.ArticleStock,
                              CategoryID = p.CategoryId,
                              VendorID = p.VendorId,
                              ProductID = p.ProductId,
                              SubCategoryID = p.SubCategoryId,
                              ArticleName = pt.ArticleName,
                              ArticleShortText = pt.ArticleShortText,
                              ArticleFeaturesOne = pt.ArticleFeaturesOne,
                              ArticleFeaturesTwo = pt.ArticleFeaturesTwo,
                              ArticleFeaturesThree = pt.ArticleFeaturesThree,
                              ArticleFeaturesFour = pt.ArticleFeaturesFour,
                              ImageId = i.ImageId,
                              ArticleImgPath = i.ImagePath + i.ImageName,
                              ArticleGuid = p.ArticleGuid,
                              LangCode = pt.LangCode,
                              ISTranslated = pt.ISTranslated,
                              ISActive = p.ISActive,
                              ISCampaign = p.ISCampaign
                          };

            IEnumerable<ArticlesViewModel> vModel = artList.ToList();
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryName", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductName", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryName", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorName", "VendorName");


            return View(vModel.FirstOrDefault());

        }
        public IActionResult Index(string vendor, string category, string product, string subcategory)//, int getVendorID, int getCategoryID, string getProductID, int getsubProductID)
        {

            var artList = from p in _context.Articles
                          where p.ISCampaign == true
                          join i in _context.Images on p.ArticleGuid equals i.ArticleGuid
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }
                          //where p._Vendor.VendorName == vendor || string.IsNullOrEmpty(vendor)
                          //where p._Category.CategoryName == category || string.IsNullOrEmpty(category)
                          //where p._Product.ProductName == product || string.IsNullOrEmpty(product)

                          select new ArticlesViewModel
                          {
                              ArticleId = p.ArticleId,
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice,
                              ArticleStock = p.ArticleStock,
                              ArticleName = pt.ArticleName,
                              ArticleShortText = pt.ArticleShortText,
                              ArticleFeaturesOne = pt.ArticleFeaturesOne,
                              ArticleFeaturesTwo = pt.ArticleFeaturesTwo,
                              ArticleFeaturesThree = pt.ArticleFeaturesThree,
                              ArticleFeaturesFour = pt.ArticleFeaturesFour,
                              ArticleImgPath = i.ImagePath + i.ImageName,
                          };

            IEnumerable<ArticlesViewModel> vModel = artList.ToList();

            return View(vModel);
        }
        // GET: Article/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artList = from p in _context.Articles
                          join i in _context.Images on p.ArticleGuid equals i.ArticleGuid
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }
                          where p.ArticleId == id
                          select new ArticlesViewModel
                          {
                              ArticleId = p.ArticleId,
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice,
                              ArticleStock = p.ArticleStock,
                              CategoryID = p.CategoryId,
                              VendorID = p.VendorId,
                              ProductID = p.ProductId,
                              SubCategoryID = p.SubCategoryId,
                              ArticleName = pt.ArticleName,
                              ArticleShortText = pt.ArticleShortText,
                              ArticleFeaturesOne = pt.ArticleFeaturesOne,
                              ArticleFeaturesTwo = pt.ArticleFeaturesTwo,
                              ArticleFeaturesThree = pt.ArticleFeaturesThree,
                              ArticleFeaturesFour = pt.ArticleFeaturesFour,
                              ImageId = i.ImageId,
                              ArticleImgPath = i.ImagePath + i.ImageName,
                              ArticleGuid = p.ArticleGuid,
                              LangCode = pt.LangCode,
                              ISTranslated = pt.ISTranslated,
                              ISActive = p.ISActive,
                              ISCampaign = p.ISCampaign
                          };

            IEnumerable<ArticlesViewModel> vModel = artList.ToList();
            if (vModel == null)
            {
                return NotFound();
            }
            //ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            //ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            //ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            //ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            return View(vModel.SingleOrDefault());
        }

        public IActionResult Contact([FromServices]IDateTime _datetime)
        {
            ContactViewModel vmodel = new ContactViewModel();
            vmodel.CurrentDateAndTime = _datetime.Now.ToString();
            vmodel.id = 0;
            List<string> list = new List<string>();
            list.Add("Andreas");
            list.Add("Tjorven");
            vmodel.Names = list;

            return View(vmodel);
        }

        public IActionResult Error()
        {
            return View();
        }

        // GET: /ShoppingCart/
        public async Task<IActionResult> CartIndex()
        {
            var cart = ShoppingCart.GetCart(_context, HttpContext); // gets all items from context.CartItems

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = await cart.GetCartItems(),
                CartTotal = await cart.GetTotal()
            };

            // Return the view
            return View("ShoppingCart", viewModel);
        }
        public async Task<IActionResult> AddToCart(int id, CancellationToken requestAborted)
        {
            // Retrieve the album from the database
            var addedArticle = await _context.Articles
                .SingleAsync(article => article.ArticleId == id);

            var addedArticleName = await _context.ArticleTranslations
               .SingleAsync(artT => artT.ArticleId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(_context, HttpContext);

            await cart.AddToCart(addedArticle, addedArticleName);

            await _context.SaveChangesAsync(requestAborted);
            _logger.LogInformation("Article {0} was added to the cart.", addedArticle.ArticleId);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

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
                .Include(c => c.ArticleId)
                .SingleOrDefaultAsync();

            string message;
            int itemCount;
            if (cartItem != null)
            {
                // Remove from cart
                itemCount = cart.RemoveFromCart(id);
                await _context.SaveChangesAsync(requestAborted);
                string removed = (itemCount > 0) ? " 1 copy of " : string.Empty;
                message = removed + cartItem.ArticleTranslation.ArticleName + " has been removed from your shopping cart.";
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

            return Json(results);
        }
        //private bool ArticleModelExists(int id)
        //{
        //    return _context.Articles.Any((System.Linq.Expressions.Expression<Func<Articles, bool>>)(e => e.ArticleId == id));
        //}
        public async Task<IActionResult> CartDetails(
            [FromServices] IMemoryCache cache,
            int id)
        {
            var cacheKey = string.Format("album_{0}", id);
            //Articles article;
            ArticleTranslation artT;
            if (!cache.TryGetValue(cacheKey, out artT))
            {
                artT = await _context.ArticleTranslations                                        
                                .Where(a => a.ArticleId == id)
                                .Include(a => a.ArticleName)
                                .FirstOrDefaultAsync();

            }

            if (artT == null)
            {
                return NotFound();
            }

            return View(artT);
        }
    }
}