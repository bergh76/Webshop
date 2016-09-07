using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webshop.Models;

namespace Webshop.Controllers
{
    public class ArticleController : Controller
    {
        private readonly WebShopRepository _context;

        public ArticleController(WebShopRepository context)
        {
            _context = context;    
        }

        // GET: Article
        public async Task<IActionResult> Index()
        {
            var webShopRepository = _context.Articles.Include(a => a.Category).Include(a => a.Product).Include(a => a.SubCategory).Include(a => a.Vendor);
            return View(await webShopRepository.ToListAsync());
        }

        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ID == id);
            if (articleModel == null)
            {
                return NotFound();
            }

            return View(articleModel);
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategoryies, "ID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors, "ID", "VendorName");
            return View();
        }

        // POST: Article/Create
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

            ViewData["Vendors"] = new SelectList(_context.Vendors, "ID", "VendorName", articleModel.VendorID);
            ViewData["Products"] = new SelectList(_context.Products, "ID", "ProductName", articleModel.ProductID);
            ViewData["Categories"] = new SelectList(_context.Categories, "ID", "CategoryName", articleModel.CategoryID);
            ViewData["SubCategories"] = new SelectList(_context.SubCategoryies, "ID", "SubCategoryName", articleModel.SubCategoryID);
            return View(articleModel);
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ID == id);
            if (articleModel == null)
            {
                return NotFound();
            }
            ViewData["Vendors"] = new SelectList(_context.Vendors, "ID", "VendorName", articleModel.VendorID);
            ViewData["Products"] = new SelectList(_context.Products, "ID", "ProductName", articleModel.ProductID);
            ViewData["Categories"] = new SelectList(_context.Categories, "ID", "CategoryName", articleModel.CategoryID);
            ViewData["SubCategories"] = new SelectList(_context.SubCategoryies, "ID", "SubCategoryName", articleModel.SubCategoryID);
            return View(articleModel);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")] ArticleModel articleModel)
        {
            if (id != articleModel.ID)
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
                    if (!ArticleModelExists(articleModel.ID))
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
            ViewData["Vendors"] = new SelectList(_context.Vendors, "ID", "VendorName", articleModel.VendorID);
            ViewData["Products"] = new SelectList(_context.Products, "ID", "ProductName", articleModel.ProductID);
            ViewData["Categories"] = new SelectList(_context.Categories, "ID", "CategoryName", articleModel.CategoryID);
            ViewData["SubCategories"] = new SelectList(_context.SubCategoryies, "ID", "SubCategoryName", articleModel.SubCategoryID);
            return View(articleModel);
        }

        // GET: Article/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ID == id);
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
            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ID == id);
            _context.Articles.Remove(articleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ArticleModelExists(int id)
        {
            return _context.Articles.Any(e => e.ID == id);
        }
    }
}
