using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webshop.HelperClasses;
using Webshop.Interfaces;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.Services;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private WebShopRepository _context;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ILogger<HomeController> _logger;
        private static string _iso;
        private static decimal _curr;
        public HomeController([FromServices]FixerIO fixer, WebShopRepository context, IStringLocalizer<HomeController> localizer, ILogger<HomeController> logger)
        {
            _iso = new RegionInfo(CultureInfo.CurrentUICulture.Name).ISOCurrencySymbol;
            _curr = FixerIO.GetUDSToRate(_iso);
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

        public async Task<IActionResult> Index([FromServices] WebShopRepository dbContext)
        {
            ViewData["CategoryID"] = new SelectList(dbContext.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(dbContext.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(dbContext.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(dbContext.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            var artList = from p in _context.Articles
                          where  p.ISCampaign == true
                          join i in _context.Images on p.ArticleGuid equals i.ArticleGuid
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }

                          select new ArticlesViewModel
                          {
                              ArticleId = p.ArticleId,
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice / _curr,
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

            IEnumerable<ArticlesViewModel> vModel = await artList.ToListAsync();
            return View(vModel);
        }

        [HttpGet]
        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
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

            IEnumerable<ArticlesViewModel> vModel = await artList.ToListAsync();
            if (vModel == null)
            {
                return NotFound();
            }
            return View(vModel.SingleOrDefault());
        }

        //[AjaxOnly]
        public async Task<IActionResult> SearchArticles(int vendor, int category, int product, int subproduct)
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
                          join i in _context.Images on p.ArticleGuid equals i.ArticleGuid
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }
                          select new ArticlesViewModel
                          {
                              ArticleId = p.ArticleId,
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice,
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

        public IActionResult _SearchBar()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            return View();
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
    }
}