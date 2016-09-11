using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webshop.Models;

namespace Webshop.Views
{
    public class ArticleModelsController : Controller
    {
        private readonly WebShopRepository _context;

        public ArticleModelsController(WebShopRepository context)
        {
            _context = context;    
        }

        // GET: ArticleModels
        public async Task<IActionResult> Index(
        //{
        //    var webShopRepository = _context.Articles.Include(a => a.Category).Include(a => a.Product).Include(a => a.SubCategory).Include(a => a.Vendor);
        //    return View(await webShopRepository.ToListAsync());
        //}
        string dropdownVendor, string dropdownProduct, string dropdownCategory, string dropdownSubCategory, int vendorID, int categoryID, string productID, int subProductID)
        {
            // PERHAPS CREATE A BUSINESSLOGICCLASS //
            IEnumerable<ArticleModel> artList = new List<ArticleModel>();
            //var isCampaign = await _context.Article.Where(x => x.ISCampaign == true).ToListAsync();
            //var outCampaign = isCampaign.Count();
            //if (outCampaign > 0)
            //{
            //    artList = isCampaign.ToList();
            //    return View(artList);
            //}
            var vendorList = new List<string>();
            var all = await _context.Articles.ToListAsync();
            ///<summary>
            ///Gets all the MANUFACTURES in the database
            /// </summary>

            var vndrQry = from v in _context.Vendors
                          orderby v.VendorName
                          select v.VendorName;

            ViewData["dropdownVendor"] = new SelectList(vendorList);
            vendorList.AddRange(vndrQry.Distinct());
            var vendor = from v in _context.Vendors
                         select v;

            ///<summary>
            ///Gets all the CATEGORIES in the database
            /// </summary>
            var catQry = from c in _context.Categories
                         orderby c.CategoryName
                         select c.CategoryName;

            var catList = new List<string>();
            ViewData["dropdownCategory"] = new SelectList(catList);
            catList.AddRange(catQry.Distinct());
            var category = from c in _context.Categories
                           select c;

            ///<summary>
            ///Gets all the PRODUCTTYPE in the database
            /// </summary>
            var prdctQry = from p in _context.Products
                           orderby p.ProductName
                           select p.ProductName;

            var prdctList = new List<string>();
            ViewData["dropdownProduct"] = new SelectList(prdctList);
            prdctList.AddRange(prdctQry.Distinct());
            var product = from p in _context.Products
                          select p;

            ///<summary>
            ///Gets all the SUBPRODUCTLIST in the database
            /// </summary>
            var subPrdctQry = from s in _context.SubCategories
                              orderby s.SubCategoryName
                              select s.SubCategoryName;

            var subPrdctList = new List<string>();
            ViewData["dropdownSubCategory"] = new SelectList(subPrdctList);
            subPrdctList.AddRange(subPrdctQry.Distinct());

            var subProduct = from s in _context.SubCategories
                             select s;

            vendor = vendor.Where(r => r.VendorName.Contains(dropdownVendor));
            var getVendorID = vendor.Where(x => x.VendorName == dropdownVendor).Select(x => x.VendorID).FirstOrDefaultAsync();
            vendorID = await getVendorID;
            var vID = vendorID;

            category = category.Where(r => r.CategoryName.Contains(dropdownCategory));
            var getCategoryID = category.Where(x => x.CategoryName == dropdownCategory).Select(x => x.CategoryID).FirstOrDefaultAsync();
            categoryID = await getCategoryID;
            var cID = categoryID;

            product = product.Where(r => r.ProductName.Contains(dropdownProduct));
            var getProductID = product.Where(x => x.ProductName == dropdownProduct).Select(x => x.ProductID).FirstOrDefaultAsync();
            productID = await getProductID;
            var pID = productID;

            subProduct = subProduct.Where(s => s.SubCategoryName.Contains(dropdownSubCategory));
            var getsubProductID = subProduct.Where(x => x.SubCategoryName == dropdownSubCategory).Select(x => x.SubCategoryID).FirstOrDefaultAsync();
            subProductID = await getsubProductID;
            var spID = subProductID;

            artList = from a in _context.Articles
                      orderby a.ArticlePrice, a.ArticleName ascending
                      where a.VendorID == vID || string.IsNullOrEmpty(dropdownVendor)
                      where a.CategoryID == cID || string.IsNullOrEmpty(dropdownCategory)
                      where a.ProductID == pID || string.IsNullOrEmpty(dropdownProduct)
                      where a.SubCategoryID == spID || string.IsNullOrEmpty(dropdownSubCategory)
                      select a;

            while (artList.Count() == 0)
            {
                ViewBag.NoHit = "Din sökning gav inga resultat";
                return View(artList);
            }

            return View(artList);

        }

// GET: ArticleModels/Details/5
public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            if (articleModel == null)
            {
                return NotFound();
            }

            return View(articleModel);
        }

        // GET: ArticleModels/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID");
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "ID");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "ID", "ID");
            ViewData["VendorID"] = new SelectList(_context.Vendors, "ID", "ID");
            return View();
        }

        // POST: ArticleModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")] ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID", articleModel.CategoryID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "ID", articleModel.ProductID);
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "ID", "ID", articleModel.SubCategoryID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "ID", "ID", articleModel.VendorID);
            return View(articleModel);
        }

        // GET: ArticleModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            if (articleModel == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID", articleModel.CategoryID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "ID", articleModel.ProductID);
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "ID", "ID", articleModel.SubCategoryID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "ID", "ID", articleModel.VendorID);
            return View(articleModel);
        }

        // POST: ArticleModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")] ArticleModel articleModel)
        {
            if (id != articleModel.ArticleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleModelExists(articleModel.ArticleID))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID", articleModel.CategoryID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "ID", articleModel.ProductID);
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories, "ID", "ID", articleModel.SubCategoryID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "ID", "ID", articleModel.VendorID);
            return View(articleModel);
        }

        // GET: ArticleModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            if (articleModel == null)
            {
                return NotFound();
            }

            return View(articleModel);
        }

        // POST: ArticleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            _context.Articles.Remove(articleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ArticleModelExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
        }
    }
}
