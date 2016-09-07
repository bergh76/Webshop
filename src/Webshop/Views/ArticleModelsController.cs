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
        public async Task<IActionResult> Index()
        {
            var webShopRepository = _context.Articles.Include(a => a.Category).Include(a => a.Product).Include(a => a.SubCategory).Include(a => a.Vendor);
            return View(await webShopRepository.ToListAsync());
        }

        // GET: ArticleModels/Details/5
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

        // GET: ArticleModels/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID");
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "ID");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategoryies, "ID", "ID");
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
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategoryies, "ID", "ID", articleModel.SubCategoryID);
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

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ID == id);
            if (articleModel == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID", articleModel.CategoryID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "ID", articleModel.ProductID);
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategoryies, "ID", "ID", articleModel.SubCategoryID);
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "ID", articleModel.CategoryID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ID", "ID", articleModel.ProductID);
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategoryies, "ID", "ID", articleModel.SubCategoryID);
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

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ID == id);
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
