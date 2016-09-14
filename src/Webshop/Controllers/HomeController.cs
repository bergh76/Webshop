using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Interfaces;
using Webshop.Models;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        private WebShopRepository _context;
        private readonly IStringLocalizer<HomeController> _localizer;
        public HomeController(WebShopRepository context, IStringLocalizer<HomeController> localizer)
        {
            _context = context;
            _localizer = localizer;
            //_datetime = datetime;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }


        public async Task<IActionResult> Index(string dropdownVendor, string dropdownProduct, string dropdownCategory, string dropdownSubCategory, int vendorID, int categoryID, string productID, int subProductID)
        {
            //PERHAPS CREATE A BUSINESSLOGICCLASS //
            IEnumerable<Articles> artList = new List<Articles>();

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
            //ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            //ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            //ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            //ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");

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

            //var webShopRepository = _context.Articles.Include(a => a.Category).Include(a => a.Product).Include(a => a.SubCategory).Include(a => a.Vendor);
            var isCampaign = artList.Where(x => x.ISCampaign == true);
            var outCampaign = isCampaign.Count();
            if (outCampaign > 10)
            {
                return View(isCampaign.ToList());
            }
            while (artList.Count() != 0)
            {
                return View(artList);
            }
            ViewBag.NoHit = "Din sökning gav inga resultat";
            return View(artList);
            //var isCampaign = await webShopRepository.Where(x => x.ISCampaign == true).ToListAsync();
            //var outCampaign = isCampaign.Count();
            //if (outCampaign > 0)
            //{                
            //    return View(isCampaign.ToList());
            //}
            //if(searchOut.Count() == 0)
            //{
            //    ViewBag.NoHit = "Din sökning gav inga resultat";
            //    return View(webShopRepository.ToListAsync());
            //}
            //if (string.IsNullOrEmpty(search))
            //{
            //    //ViewBag.NoHit = "Din sökning gav inga resultat";
            //    return View(webShopRepository.ToListAsync());
            //}
            //return View(await webShopRepository.ToListAsync());
        }
        // GET: Article/Details/5
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

            //BreadCrumTracker bc = new BreadCrumTracker(_context, id);
            //await bc.GetTracker();
            return View(articleModel);
        }

        public IActionResult About(int id, string name, string lang)
        {
            ViewData["Message"] = lang +" "+  id + " " + name;

            return View();
        }

        public IActionResult Contact([FromServices]IDateTime _datetime)
        {
            ContactViewModel vmodel = new ContactViewModel();
            vmodel.CurrentDateAndTime = _datetime.Now.ToString();
            vmodel.id = 0;
            List<string> list = new List<string>();
            list.Add("Andreas");
            list.Add("Tjorven");
            vmodel.Names = list;

            return View(vmodel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
