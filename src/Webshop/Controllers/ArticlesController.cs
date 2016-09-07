using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Webshop.Controllers
{
    public class ArticlesController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(string vendorname)
        {
            // PERHAPS CREATE A BUSINESSLOGICCLASS //
            //IEnumerable<VendorModel> result = new List<VendorModel>();
            //var vList = new List<string>();
            //var vendor1 = new VendorModel()
            //{
            //    ID = 0,
            //    VendorID = 123456789,
            //    VendorName = "Sony",
            //    VendorWebPage = "",
            //    ISActive = true
            //};
            //var vendor2 = new VendorModel()
            //{
            //    ID = 1,
            //    VendorID = 987654321,
            //    VendorName = "Rotel",
            //    VendorWebPage = "",
            //    ISActive = true

            //};
            //var vendor3 = new VendorModel()
            //{
            //    ID = 2,
            //    VendorID = 0918273645,
            //    VendorName = "Densen",
            //    VendorWebPage = "",
            //    ISActive = false
            //};
            //vList.Add(vendor1.VendorName);
            //vList.Add(vendor2.VendorName);
            //vList.Add(vendor3.VendorName);
            //var sort = from l in vList
            //           orderby l ascending
            //           select l;
            //ViewData["dropdownVendor"] = new SelectList(sort);
            return View();
        }
    }
}