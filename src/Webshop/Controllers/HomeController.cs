﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Interfaces;
using Webshop.Models;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        private WebShopRepository _context;
        private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(WebShopRepository context, IStringLocalizer<HomeController> localizer)
        {
            _context = context;
            _localizer = localizer;
            //_datetime = datetime;
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

        public IActionResult Index(string vendor, string category, string product, string subcategory)//, int getVendorID, int getCategoryID, string getProductID, int getsubProductID)
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryName", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductName", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryName", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorName", "VendorName");


            var artList = from p in _context.Articles
                          //where p.ISCampaign == true
                          join i in _context.Images on p.ArticleGuid equals i.ArticleGuid
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }
                          //where p._Vendor.VendorName == vendor || string.IsNullOrEmpty(vendor)
                          //where p._Category.CategoryName == category || string.IsNullOrEmpty(category)
                          //where p._Product.ProductName == product || string.IsNullOrEmpty(product)

                          select new ArticlesViewModel
                          {
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice,
                              ArticleStock = p.ArticleStock,
                              //ISActive = p.ISActive,
                              //ISCampaign = p.ISCampaign,
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles.SingleOrDefaultAsync((System.Linq.Expressions.Expression<Func<Articles, bool>>)(m => m.ArticleId == id));
            if (articleModel == null)
            {
                return NotFound();
            }

            //BreadCrumTracker bc = new BreadCrumTracker(_context, id);
            //await bc.GetTracker();
            return View(articleModel);
        }

        public IActionResult About(int id, string name, string lang)
        {
            ViewData["Message"] = lang + " " + id + " " + name;

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


        //private bool ArticleModelExists(int id)
        //{
        //    return _context.Articles.Any((System.Linq.Expressions.Expression<Func<Articles, bool>>)(e => e.ArticleId == id));
        //}
    }
}