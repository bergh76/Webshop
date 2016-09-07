using System.Collections.Generic;
using Webshop.Models;

namespace Webshop.ViewModel
{
    public class SubCatViewModel
    {

        public SubCategory SubCatObject { get; set; }
        public IEnumerable<SubCategory> SubCatList { get; set; }

        //public ImageViewModel(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public ImageViewModel(object o)
        //{
        //   o = _context.Image.ToList();
        //}
    }
}