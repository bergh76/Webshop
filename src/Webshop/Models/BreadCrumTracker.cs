using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models
{
    public class BreadCrumTracker
    {
        private WebShopRepository _context;
        private int? id;

        public BreadCrumTracker(WebShopRepository _context, int? id)
        {
            this._context = _context;
            this.id = id;
        }

        //public int ArticleID { get; set; }
        public string VendorName { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string SubcategoryName { get; set; }

        internal async Task GetTracker()
        {
            var vID = await _context.Articles.Where(x => x.ArticleID == id).Select(x => x.VendorID).FirstOrDefaultAsync();
            var cID = await _context.Articles.Where(x => x.ArticleID == id).Select(x => x.CategoryID).FirstOrDefaultAsync();
            var pID = await _context.Articles.Where(x => x.ArticleID == id).Select(x => x.ProductID).FirstOrDefaultAsync();
            var sID = await _context.Articles.Where(x => x.ArticleID == id).Select(x => x.SubCategoryID).FirstOrDefaultAsync();
            VendorName = _context.Vendors.Where(x => x.VendorID == vID).Select(x => x.VendorName).FirstOrDefault();
            CategoryName = _context.Categories.Where(x => x.CategoryID == cID).Select(x => x.CategoryName).FirstOrDefault();
            ProductName = _context.Products.Where(x => x.ProductID == pID).Select(x => x.ProductName).FirstOrDefault();
            SubcategoryName = _context.SubCategories.Where(x => x.SubCategoryID == sID).Select(x => x.SubCategoryName).FirstOrDefault();

        }
    }
}
