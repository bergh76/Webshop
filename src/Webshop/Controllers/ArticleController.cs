using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Webshop.Interfaces;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.Services;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly IDateTime _datetime;
        private readonly WebShopRepository _context;
        private readonly IStringLocalizer<ArticleController> _localizer;
        private readonly IHostingEnvironment _hostEnvironment; // service that provides some useful environment information such as the current file path
        private readonly ILogger<ArticleController> _logger;

        private static string _iso;
        private static decimal _curr;
        public ArticleController([FromServices]FixerIO fixer, [FromServices] WebShopRepository context,IDateTime datetime, IHostingEnvironment hostEnvironment, IStringLocalizer<ArticleController> localizer, ILogger<ArticleController> logger)
        {
            _iso = new RegionInfo(CultureInfo.CurrentUICulture.Name).ISOCurrencySymbol;
            _curr = FixerIO.GetUDSToRate(_iso);
            _context = context;
            _hostEnvironment = hostEnvironment;
            _localizer = localizer;
            _logger = logger;
            _datetime = datetime;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            if (ModelState.IsValid)
            {
                var artList = from p in _context.Articles
                                  //where p.ISCampaign == true
                              join i in _context.Images on p.ArticleId equals i.ArtikelId
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
            return View();

        }

        // GET: Article/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artList = from p in _context.Articles
                          join i in _context.Images on p.ArticleId equals i.ArtikelId
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            //ViewData["LangCode"] = new SelectList(_context.Languages, "ID", "LangCode");
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            ViewData["Vendor"] = ArticleBusinessLayer.vendorMessage;
            ViewData["Category"] = ArticleBusinessLayer.categoryMessage;
            ViewData["Product"] = ArticleBusinessLayer.productMessage;
            ViewData["SubCategory"] = ArticleBusinessLayer.subcatMessage;
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artList = from p in _context.Articles
                          join i in _context.Images on p.ArticleId equals i.ArtikelId
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, IFormFile file, IFormCollection form, [Bind("Translation,ArticleId,ArticleName,ArticleNumber,ArticleAddDate,ArticleFeaturesOne,ArticleFeaturesTwo,ArticleFeaturesThree,ArticleFeaturesFour,ArticleGuid,ArticlePrice,ArticleShortText,ArticleStock,CategoryId,ISActive,ISCampaign,ProductId,ProductImgPathID,SubCategoryId,VendorId,ArticleImgPath,ImageId,LangCode")]Articles article, ArticleTranslation artTrans, ArticleBusinessLayer newArticle, ImageModel img)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await newArticle.EditArticle(_context, _datetime, article, artTrans,  img, _hostEnvironment, id, file, form);
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
            PopoulateDropdown(article);
            return View(article);
        }

        public void PopoulateDropdown(Articles article)
        {
            ViewData["Vendors"] = new SelectList(_context.Vendors, "VendorID", "VendorName", article.VendorId);
            ViewData["Products"] = new SelectList(_context.Products, "ProductID", "ProductName", article.ProductId);
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", article.CategoryId);
            ViewData["SubCategories"] = new SelectList(_context.SubCategories, "SubCategoryID", "SubCategoryName", article.SubCategoryId);

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var artList = from p in _context.Articles
                          join i in _context.Images on p.ArticleId equals i.ArtikelId
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleId == id);
                var artTrans = await _context.ArticleTranslations.Where(d => d.ArticleId == id).ToListAsync();
                foreach (ArticleTranslation item in artTrans)
                {
                    _context.ArticleTranslations.Remove(item);
                }
                _context.Articles.Remove(article);
                try
                {
                    await _context.SaveChangesAsync();
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NewArticle(IFormFile file, IFormCollection form, int VendorID, int ProductID, int CategoryID, int SubCategoryID, [Bind("ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")]Articles article, ArticleTranslation artTranslate, ArticleBusinessLayer add)
        {
            if (ModelState.IsValid)
            {
                await add.AddArticle(_context, article, artTranslate, _datetime, file, form,_hostEnvironment, VendorID, ProductID, CategoryID, SubCategoryID);
                return RedirectToAction("Create");
            }
            return View(article);
        }

        // GET: Article/Create
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> Translate(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            else if (_iso != "SEK")
            {
                return View("Error");
            }
            var artList = from p in _context.Articles
                          join i in _context.Images on p.ArticleId equals i.ArtikelId
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
                              ArticleImgPath = i.ImagePath + i.ImageName
                          };

            IEnumerable<ArticlesViewModel> vModel = await artList.ToListAsync();
            if (vModel == null)
            {
                return View("Error");
            }
            return View(vModel.FirstOrDefault());
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
            _logger.LogInformation("Article {0} was translated not successfully:", id);
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
        public async Task<IActionResult> NewVendor([Bind("VendorID, VendorName, VendorWebPage, ISActive")]VendorModel vendor, ArticleBusinessLayer artBLL)
        {
            if (ModelState.IsValid)
            {
                await artBLL.AddNewVendor(_context, vendor);
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
        public async Task<IActionResult> NewProduct([Bind("ProductID, ProductName, ISActive")] ProductModel product, ArticleBusinessLayer artBLL)
        {

            if (ModelState.IsValid)
            {
                await artBLL.AddNewProduct(_context, product);
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
        public async Task<IActionResult> NewCategory([Bind("CategoryID, CategoryName, ISActive")] CategoryModel category, ArticleBusinessLayer artBLL)
        {

            if (ModelState.IsValid)
            {
                await artBLL.AddNewCategory(_context,category);
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
        public async Task<IActionResult> NewSubCategory([Bind("SubCategoryID, SubCategoryName, ISActive")] SubCategoryModel subCategory, ArticleBusinessLayer artBLL)
        {

            if (ModelState.IsValid)
            {
                await artBLL.AddNewSubProduct(_context, subCategory);
                return RedirectToAction("Create");
            }
            return View(subCategory);
        }

        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

        public IActionResult Error()
        {
            return View();
        }
        private bool ArticleModelExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}

