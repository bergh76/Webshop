﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.Services;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class ArticleController : Controller
    {
        private readonly WebShopRepository _context;
        private readonly IStringLocalizer<ArticleController> _localizer;
        private readonly IHostingEnvironment _hostEnvironment; // service that provides some useful environment information such as the current file path
        private static string _iso;
        private static decimal _curr;
        public ArticleController([FromServices]FixerIO fixer, WebShopRepository context, IHostingEnvironment hostEnvironment, IStringLocalizer<ArticleController> localizer)
        {
            _iso = new RegionInfo(CultureInfo.CurrentUICulture.Name).ISOCurrencySymbol;
            _curr = FixerIO.GetUDSToRate(_iso);
            _context = context;
            _hostEnvironment = hostEnvironment;
            _localizer = localizer;
        }
        // GET: Article

        //public async Task<IActionResult> Index()
        //{
        //    var webShopRepository = _context.Articles.Include(a => a.Category).Include(a => a.Product).Include(a => a.SubCategory).Include(a => a.Vendor);
        //    return View(await webShopRepository.ToListAsync());
        //}


        public async Task<IActionResult> Index(string vendor, string category, string product, string subcategory)
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");

            var artList = from p in _context.Articles
                              //where p.ISCampaign == true
                          join i in _context.Images on p.ArticleGuid equals i.ArticleGuid
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }
                          where p.ISActive == true
                          select new ArticlesViewModel
                          {
                              ArticleId = p.ArticleId,
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice / _curr,
                              ArticleStock = p.ArticleStock,
                              ArticleName = pt.ArticleName,
                              ArticleShortText = pt.ArticleShortText,
                              ArticleFeaturesOne = pt.ArticleFeaturesOne,
                              ArticleFeaturesTwo = pt.ArticleFeaturesTwo,
                              ArticleFeaturesThree = pt.ArticleFeaturesThree,
                              ArticleFeaturesFour = pt.ArticleFeaturesFour,
                              ArticleImgPath = i.ImagePath + i.ImageName,
                          };

            IEnumerable<ArticlesViewModel> vModel = await artList.ToListAsync();

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

            IEnumerable<ArticlesViewModel> vModel = artList.ToList();
            if (vModel == null)
            {
                return NotFound();
            }
            return View(vModel.SingleOrDefault());
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            //ViewData["LangCode"] = new SelectList(_context.Languages, "ID", "LangCode");
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            return View();
        }

        // POST: Article/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")] Articles articleModel, ArticleTranslation artTranslate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleModel);
                _context.Add(artTranslate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["Vendors"] = new SelectList(_context.Vendors, "VendorID", "VendorName", articleModel.VendorId);
            ViewData["Products"] = new SelectList(_context.Products, "ProductID", "ProductName", articleModel.ProductId);
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", articleModel.CategoryId);
            ViewData["SubCategories"] = new SelectList(_context.SubCategories, "SubCategoryID", "SubCategoryName", articleModel.SubCategoryId);
            return View(articleModel);
        }

        // GET: Article/Edit/5
        public IActionResult Edit(int? id)
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

            IEnumerable<ArticlesViewModel> vModel = artList.ToList();
            if (vModel == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            return View(vModel.SingleOrDefault());
        }


        // POST: Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile file, IFormCollection form, [Bind("Translation,ArticleId,ArticleName,ArticleNumber,ArticleAddDate,ArticleFeaturesOne,ArticleFeaturesTwo,ArticleFeaturesThree,ArticleFeaturesFour,ArticleGuid,ArticlePrice,ArticleShortText,ArticleStock,CategoryId,ISActive,ISCampaign,ProductId,ProductImgPathID,SubCategoryId,VendorId,ArticleImgPath,ImageId,LangCode")]Articles article, ArticleTranslation artTrans, ArticleBusinessLayer newArticle)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await newArticle.EditArticle(article, artTrans, _context, _hostEnvironment, id, file, form);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleModelExists(article.ArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["Vendors"] = new SelectList(_context.Vendors, "VendorID", "VendorName", article.VendorId);
            ViewData["Products"] = new SelectList(_context.Products, "ProductID", "ProductName", article.ProductId);
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", article.CategoryId);
            ViewData["SubCategories"] = new SelectList(_context.SubCategories, "SubCategoryID", "SubCategoryName", article.SubCategoryId);
            return View(article);
        }


        public async Task<IActionResult> Delete(int? id)
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
            if (vModel == null)
            {
                return NotFound();
            }
            return View(vModel.SingleOrDefault());
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleId == id);
            var artTrans = await _context.ArticleTranslations.SingleOrDefaultAsync(d => d.ArticleId == id);
            _context.ArticleTranslations.Remove(artTrans);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        // GET: Articles/Create
        public IActionResult NewArticle()
        {
            //ViewData["LangCode"] = new SelectList(_context.Languages, "ID", "LangCode");
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "VendorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> NewArticle(IFormFile file, IFormCollection form, int VendorID, int ProductID, int CategoryID, int SubCategoryID, [Bind("ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")]Articles article, ArticleTranslation artTranslate, ArticleBusinessLayer add)
        {

            if (ModelState.IsValid)
            {
                await add.AddArticle(file, form, _context, article, artTranslate, _hostEnvironment, VendorID, ProductID, CategoryID, SubCategoryID);
                return RedirectToAction("Create");
            }
            return View(article);
        }

        // GET: Article/Create
        public IActionResult Translate(int? id)
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
                              ArticleId = pt.ArticleId,
                              ArticleName = pt.ArticleName,
                              ArticleNumber = p.ArticleNumber,
                              ArticleShortText = pt.ArticleShortText,
                              ArticleFeaturesOne = pt.ArticleFeaturesOne,
                              ArticleFeaturesTwo = pt.ArticleFeaturesTwo,
                              ArticleFeaturesThree = pt.ArticleFeaturesThree,
                              ArticleFeaturesFour = pt.ArticleFeaturesFour,
                              ArticlePrice = p.ArticlePrice / _curr,
                              ArticleStock = p.ArticleStock,
                              LangCode = pt.LangCode,
                              ISTranslated = pt.ISTranslated,
                              ImageId = i.ImageId,
                              ArticleImgPath = i.ImagePath + i.ImageName,
                              ArticleGuid = p.ArticleGuid,
                              ISActive = p.ISActive,
                              ISCampaign = p.ISCampaign
                          };

            IEnumerable<ArticlesViewModel> vModel = artList.ToList();
            if (vModel == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            return View(vModel.SingleOrDefault());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Translate(int id, string text, string one, string two, string three, string four, ArticlesViewModel artView, ArticleTranslation artTrans, ArticleBusinessLayer add)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await add.Translate(id, _context, artView, artTrans, text, one, two, three, four);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleModelExists(artView.ArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Create");
            }
            return View();
        }
        public IActionResult NewVendor()
        {
            return View();
        }

        // POST: Admin/NewVendor
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> NewVendor([Bind("VendorID, VendorName, VendorWebPage, ISActive")]VendorModel vendor)
        {
            if (ModelState.IsValid)
            {
                var nameInput = vendor.VendorName;
                var exists = _context.Vendors.ToList().Where(x => x.VendorName == nameInput).Select(x => x.VendorName).FirstOrDefault();
                do while (nameInput == exists)
                    {
                        return RedirectToAction("Create");
                    }
                while (false);
                var v = _context.Vendors.ToList().Select(x => x.VendorID).Count();
                if (v == 0)
                {
                    int tempV = 9001;
                    vendor.VendorID = tempV;
                }
                else
                {
                    var getLastID = _context.Vendors.ToList().OrderBy(x => x.VendorID).Select(x => x.VendorID).Last();
                    vendor.VendorID = getLastID + 1;
                }
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(vendor);
        }

        public IActionResult NewProduct()
        {
            return View();
        }

        // POST: Admin/NewProduct
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> NewProduct([Bind("ProductID, ProductName, ISActive")] ProductModel product)
        {

            if (ModelState.IsValid)
            {
                var nameInput = product.ProductName;
                var exists = _context.Products.ToList().Where(x => x.ProductName == nameInput).Select(x => x.ProductName).FirstOrDefault();
                do while (nameInput == exists)
                    {
                        return RedirectToAction("Create");
                    }
                while (false);
                var p = _context.Products.ToList().Select(x => x.ProductID).Count();
                if (p == 0)
                {
                    int tempP = 4001;
                    product.ProductID = tempP;

                }
                else
                {
                    var getLastID = _context.Products.ToList().OrderBy(x => x.ProductID).Select(x => x.ProductID).Last();
                    var tempIntProdID = Convert.ToInt32(getLastID);
                    product.ProductID = tempIntProdID + 1;

                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(product);
        }

        public IActionResult NewCategory()
        {
            return View();
        }

        // POST: Admin/NewVendor
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> NewCategory([Bind("CategoryID, CategoryName, ISActive")] CategoryModel category)
        {

            if (ModelState.IsValid)
            {
                var nameInput = category.CategoryName;
                var exists = _context.Categories.ToList().Where(x => x.CategoryName == nameInput).Select(x => x.CategoryName).FirstOrDefault();
                do while (nameInput == exists)
                    {
                        return RedirectToAction("Create");
                    }
                while (false);
                var c = _context.Categories.ToList().Select(x => x.CategoryID).Count();
                if (c == 0)
                {
                    int tempC = 1010;
                    category.CategoryID = tempC;

                }
                else
                {
                    var getLastID = _context.Categories.ToList().OrderBy(x => x.CategoryID).Select(x => x.CategoryID).Last();
                    category.CategoryID = getLastID + 100;
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(category);
        }

        public IActionResult NewSubCategory()
        {
            return View();
        }

        // POST: Admin/NewSubCategory
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> NewSubCategory([Bind("SubCategoryID, SubCategoryName, ISActive")] SubCategoryModel subCategory)
        {

            if (ModelState.IsValid)
            {
                var nameInput = subCategory.SubCategoryName;
                var exists = _context.SubCategories.ToList().Where(x => x.SubCategoryName == nameInput).Select(x => x.SubCategoryName).FirstOrDefault();
                do while (nameInput == exists)
                    {
                        return RedirectToAction("Create");
                    }
                while (false);
                var s = _context.SubCategories.ToList().Select(x => x.SubCategoryID).Count();

                if (s == 0)
                {
                    int tempS = 1001;
                    subCategory.SubCategoryID = tempS;
                }

                else
                {
                    var getLastID = _context.SubCategories.ToList().OrderBy(x => x.SubCategoryID).Select(x => x.SubCategoryID).Last();
                    subCategory.SubCategoryID = getLastID + 1;
                }
                _context.Add(subCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(subCategory);
        }


        private bool ArticleModelExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}

