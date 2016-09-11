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

        public BreadCrumTracker(WebShopRepository context)
        {
            _context = context;
        }

        public BreadCrumTracker(int? id)
        {
            this.id = id;
        }

        public int ArticleID { get; set; }
        public string VendorName { get; set; }
        public string CategoryName { get; set; }
        public string ProduktName { get; set; }
        public string SubcategoryName { get; set; }

        internal Task GetTracker()
        {
            var articleModel = _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            return articleModel;
        }
    }
}
