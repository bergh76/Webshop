﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Webshop.BusinessLayers;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class ArticleController : Controller
    {
        private readonly WebShopRepository _context;
        private readonly IStringLocalizer<ArticleController> _localizer;
        private readonly IHostingEnvironment _hostEnvironment;

        public ArticleController(WebShopRepository context, IHostingEnvironment hostEnvironment, IStringLocalizer<ArticleController> localizer)
        {
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


        public IActionResult Index(string dropdownVendor, string dropdownProduct, string dropdownCategory, string dropdownSubCategory, int getVendorID, int getCategoryID, string getProductID, int getsubProductID)
        {
            var webShopRepository = _context.Articles.Include(a => a._Category).Include(a => a._Product).Include(a => a._SubCategory).Include(a => a._Vendor);
            //    return View(await webShopRepository.ToListAsync());

            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");

            getVendorID = _context.Vendors.Where(x => x.VendorName == dropdownVendor).Select(x => x.VendorID).FirstOrDefault();
            int vID = getVendorID;

            getCategoryID = _context.Categories.Where(x => x.CategoryName == dropdownCategory).Select(x => x.CategoryID).FirstOrDefault();
            var cID = getCategoryID;

            getProductID = _context.Products.Where(x => x.ProductName == dropdownProduct).Select(x => x.ProductID).FirstOrDefault();
            var pID = getProductID;

            getsubProductID = _context.SubCategories.Where(x => x.SubCategoryName == dropdownSubCategory).Select(x => x.SubCategoryID).FirstOrDefault();
            var spID = getsubProductID;

            var query = from p in _context.Articles
                        join pt in _context.ArticleTranslations on
                        new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                        equals new { pt.ArticleId, Second = pt.LangCode }
                        where p._Vendor.VendorID == vID || string.IsNullOrEmpty(dropdownVendor)
                        where p._Category.CategoryID == cID || string.IsNullOrEmpty(dropdownCategory)
                        where p._Product.ProductID == pID || string.IsNullOrEmpty(dropdownProduct)
                        where p._SubCategory.SubCategoryID == spID || string.IsNullOrEmpty(dropdownSubCategory)

                        select new ArticlesViewModel
                        {
                            ArticleID = p.ArticleId,
                            ArticlePrice = p.ArticlePrice,
                            ArticleStock = p.ArticleStock,
                            ISActive = p.ISActive,
                            ISCampaign = p.ISCampaign,
                            ArticleName = pt.ArticleName,
                            ArticleShortText = pt.ArticleShortText,
                            ArticleFeaturesOne = pt.ArticleFeaturesOne,
                            ArticleFeaturesTwo = pt.ArticleFeaturesTwo,
                            ArticleFeaturesThree = pt.ArticleFeaturesThree,
                            ArticleFeaturesFour = pt.ArticleFeaturesFour
                        };
            //          where a.CategoryID == cID || string.IsNullOrEmpty(dropdownCategory)
            //          where a.ProductID == pID || string.IsNullOrEmpty(dropdownProduct)
            //          where a.SubCategoryID == spID || string.IsNullOrEmpty(dropdownSubCategory)
            //          select a;

            //artList = from a in _context.Articles
            //          orderby a.ArticlePrice, a.ArticleName ascending
            //          where a.VendorID == vID || string.IsNullOrEmpty(dropdownVendor)
            //          where a.CategoryID == cID || string.IsNullOrEmpty(dropdownCategory)
            //          where a.ProductID == pID || string.IsNullOrEmpty(dropdownProduct)
            //          where a.SubCategoryID == spID || string.IsNullOrEmpty(dropdownSubCategory)
            //          select a;

            //var webShopRepository = _context.Articles.Include(a => a.Category).Include(a => a.Product).Include(a => a.SubCategory).Include(a => a.Vendor);
            //var isCampaign = artList.Where(x => x.ISCampaign == true);
            //var outCampaign = isCampaign.Count();
            //if (outCampaign > 10)
            //{
            //    return View(isCampaign.ToList());
            //}
            //while (artList.Count() != 0)
            //{
            //    return View(artList);
            //}
            //ViewBag.NoHit = "Din sökning gav inga resultat";
            //return View(query.ToListAsync());
            return View(query);

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
        public async Task<IActionResult> Create([Bind("ID,ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")] Articles articleModel, ArticleTranslation artTranslate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleModel);
                _context.Add(artTranslate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewData["LangCode"] = new SelectList(_context.Languages, "ID", "LangCode", artTranslate.LangCode);
            ViewData["Vendors"] = new SelectList(_context.Vendors, "VendorID", "VendorName", articleModel.VendorId);
            ViewData["Products"] = new SelectList(_context.Products, "ProductID", "ProductName", articleModel.ProductId);
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", articleModel.CategoryId);
            ViewData["SubCategories"] = new SelectList(_context.SubCategories, "SubCategoryID", "SubCategoryName", articleModel.SubCategoryId);
            return View(articleModel);
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            return View(articleModel);
        }


        // POST: Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleID,ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID,ArticleImgPath")]Articles article, EditArticleBusiness newArticle)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     newArticle.UpdateArticleData(article,_context);
                    _context.Update(article);
                    await _context.SaveChangesAsync();
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

        //public async Task<IActionResult> UpdateImage(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var articleModel = await _context.Articles.SingleOrDefaultAsync((System.Linq.Expressions.Expression<Func<Articles, bool>>)(m => m.ArticleId == id));
        //    if (articleModel == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
        //    ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
        //    ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
        //    ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
        //    return View(articleModel);
        //}


        //public async Task<IActionResult> UpdateImage(ArticleModel article, EditArticleBusiness newArticle, IFormFile file, string ext, string newFilename, string message, int id, [Bind("ArticleID,ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID,ArticleImgPath")] IFormCollection form)
        //{
        //    if (id != article.ArticleID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            //
        //            newArticle.UpdateArticleImage(article, _hostEnvironment, _context, ext, newFilename, file, form, message)
        //            _context.Update(article);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ArticleModelExists(article.ArticleID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index");
        //    }
        //}
        // GET: Article/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(articleModel);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleModel = await _context.Articles.SingleOrDefaultAsync((System.Linq.Expressions.Expression<Func<Articles, bool>>)(m => m.ArticleId == id));
            _context.Articles.Remove((Articles)articleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Search(string search)
        //{
        //    var hitlist = _context.Articles.Where((System.Linq.Expressions.Expression<Func<Articles, bool>>)(x => (bool)x.ArticleName.Contains(search)));
        //    if(hitlist.Count() == 0)
        //    {
        //        ViewBag.NoHit = "Din sökning gav inga resultat";
        //    }
        //    return RedirectToAction("Index", hitlist);
        //}

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
        //[Authorize]
        //public IActionResult NewArticle(string dropdownVendor, string dropdownProduct, string dropdownCategory, string dropdownSubCategory)
        //{
        //    ///<summary>
        //    ///Gets all the MANUFACTURES in the database
        //    /// </summary>
        //    var vndrQry = from v in _context.Vendors
        //                  orderby v.VendorName
        //                  select v.VendorName;
        //    var vendorList = new List<string>();
        //    vendorList.AddRange(vndrQry.Distinct());
        //    var vendor = from v in _context.Vendors
        //                 select v;
        //    ViewData["dropdownVendor"] = new SelectList(vendorList);

        //    ///<summary>
        //    ///Gets all the PRODUCTTYPE in the database
        //    /// </summary>
        //    var prdctQry = from p in _context.Products
        //                   orderby p.ProductName
        //                   select p.ProductName;
        //    var prdctList = new List<string>();
        //    prdctList.AddRange(prdctQry.Distinct());
        //    var procuct = from p in _context.Products
        //                  select p;
        //    ViewData["dropdownProduct"] = new SelectList(prdctList);

        //    ///<summary>
        //    ///Gets all the CATEGORIES in the database
        //    /// </summary>
        //    var catQry = from c in _context.Categories
        //                 orderby c.CategoryName
        //                 select c.CategoryName;
        //    var catList = new List<string>();
        //    catList.AddRange(catQry.Distinct());
        //    var category = from c in _context.Categories
        //                   select c;
        //    ViewData["dropdownCategory"] = new SelectList(catList);

        //    ///<summary>
        //    ///Gets all the SUBPRODUCTLIST in the database
        //    /// </summary>
        //    var subPrdctQry = from s in _context.SubCategories
        //                      orderby s.SubCategoryName
        //                      select s.SubCategoryName;
        //    var subPrdctList = new List<string>();
        //    subPrdctList.AddRange(subPrdctQry.Distinct());
        //    var subProduct = from s in _context.SubCategories
        //                     select s;
        //    ViewData["dropdownSubCategory"] = new SelectList(subPrdctList);
        //    return View();
        //}


        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult NewArticle(Articles article, ArticleTranslation artTranslate, AddArticleBusinessLayer add, IFormFile file, [Bind("ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")] IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                add.AddArticle(article, artTranslate, _context, _hostEnvironment, _localizer, file, form);
                return RedirectToAction("Create");
            }
            return View(article);
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
                    string tempP = "001";
                    product.ProductID = tempP;

                }
                else
                {
                    var getLastID = _context.Products.ToList().OrderBy(x => x.ProductID).Select(x => x.ProductID).Last();
                    var tempIntProdID = Convert.ToInt32(getLastID);
                    if (tempIntProdID < 10)
                    {
                        int newPId = tempIntProdID + 1;
                        var newProdID = String.Format("00{0}", newPId);
                        product.ProductID = newProdID;
                    }
                    else if (tempIntProdID < 100 && tempIntProdID >= 10)
                    {
                        int newPId = tempIntProdID + 1;
                        var newProdID = String.Format("0{0}", newPId);
                        product.ProductID = newProdID;
                    }
                    else
                    {
                        int newPId = tempIntProdID + 1;
                        var newProdID = String.Format("{0}", newPId);
                        product.ProductID = newProdID;
                    }
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
            return _context.Articles.Any((System.Linq.Expressions.Expression<Func<Articles, bool>>)(e => e.ArticleId == id));
        }
    }
}
