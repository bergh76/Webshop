using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Webshop.BusinessLayers;
using Webshop.Models;
using Webshop.ViewModel;

namespace Webshop.Controllers
{
    public class ArticleController : Controller
    {
        private readonly WebShopRepository _context;
        private readonly IHostingEnvironment _hostEnvironment;
        private object message;

        public ArticleController(WebShopRepository context, IHostingEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Article

        //public async Task<IActionResult> Index()
        //{
        //    var webShopRepository = _context.Articles.Include(a => a.Category).Include(a => a.Product).Include(a => a.SubCategory).Include(a => a.Vendor);
        //    return View(await webShopRepository.ToListAsync());
        //}
        public async Task<IActionResult> Index(string dropdownVendor, string dropdownProduct, string dropdownCategory, string dropdownSubCategory, int vendorID, int categoryID, string productID, int subProductID)

        {
            // PERHAPS CREATE A BUSINESSLOGICCLASS //
            IEnumerable<ArticleModel> artList = new List<ArticleModel>();
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
        }


        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            //if (articleModel == null)
            //{
            //    return NotFound();
            //}

            BreadCrumTracker bc = new BreadCrumTracker(_context, id);
            await bc.GetTracker();
            return View(bc);
        }

        // GET: Article/Create
        public IActionResult Create()
        {
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
        public async Task<IActionResult> Create([Bind("ID,ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")] ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["Vendors"] = new SelectList(_context.Vendors, "VendorID", "VendorName", articleModel.VendorID);
            ViewData["Products"] = new SelectList(_context.Products, "ProductID", "ProductName", articleModel.ProductID);
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", articleModel.CategoryID);
            ViewData["SubCategories"] = new SelectList(_context.SubCategories, "SubCategoryID", "SubCategoryName", articleModel.SubCategoryID);
            return View(articleModel);
        }

        // GET: Article/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("ArticleID,ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID,ArticleImgPath")]ArticleModel article, EditArticleBusiness newArticle)
        {
            if (id != article.ArticleID)
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
                    if (!ArticleModelExists(article.ArticleID))
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
            ViewData["Vendors"] = new SelectList(_context.Vendors, "VendorID", "VendorName", article.VendorID);
            ViewData["Products"] = new SelectList(_context.Products, "ProductID", "ProductName", article.ProductID);
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", article.CategoryID);
            ViewData["SubCategories"] = new SelectList(_context.SubCategories, "SubCategoryID", "SubCategoryName", article.SubCategoryID);
            return View(article);
        }

        public async Task<IActionResult> UpdateImage(int? id)
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
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(x => x.CategoryName), "CategoryID", "CategoryName");
            ViewData["ProductID"] = new SelectList(_context.Products.OrderBy(x => x.ProductName), "ProductID", "ProductName");
            ViewData["SubCategoryID"] = new SelectList(_context.SubCategories.OrderBy(x => x.SubCategoryName), "SubCategoryID", "SubCategoryName");
            ViewData["VendorID"] = new SelectList(_context.Vendors.OrderBy(x => x.VendorName), "VendorID", "VendorName");
            return View(articleModel);
        }


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

            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
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
            var articleModel = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleID == id);
            _context.Articles.Remove(articleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string search)
        {
            var hitlist = _context.Articles.Where(x => x.ArticleName.Contains(search)).Select(x => x.ArticleName);
            return RedirectToAction("Index",await _context.Articles.ToListAsync());
        }

        // GET: Articles/Create
        public IActionResult NewArticle()
        {
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
        public async Task<IActionResult> NewArticle(ArticleModel article, IFormFile file, string ext, string newFilename, [Bind("ArticleAddDate,ArticleFeaturesFour,ArticleFeaturesOne,ArticleFeaturesThree,ArticleFeaturesTwo,ArticleGuid,ArticleName,ArticleNumber,ArticlePrice,ArticleShortText,ArticleStock,CategoryID,ISActive,ISCampaign,ProductID,ProductImgPathID,SubCategoryID,VendorID")] IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                var date = DateTime.Now.ToLocalTime();
                int vendorID = Convert.ToInt32(form["VendorID"]);
                string vendor = _context.Vendors.Where(x => x.VendorID == vendorID).Select(x => x.VendorName).FirstOrDefault();
                article.VendorID = Convert.ToInt32(vendorID);

                int categoryID = Convert.ToInt32(form["CategoryID"]);
                string category = _context.Categories.Where(x => x.CategoryID == categoryID).Select(x => x.CategoryName).FirstOrDefault();
                article.CategoryID = categoryID;

                string productID = form["ProductID"];
                string product = _context.Products.Where(x => x.ProductID == productID).Select(x => x.ProductName).FirstOrDefault();
                article.ProductID = productID;

                int subproductID = Convert.ToInt32(form["SubCategoryID"]);
                string subproduct = _context.SubCategories.Where(x => x.SubCategoryID == subproductID).Select(x => x.SubCategoryName).FirstOrDefault();
                article.SubCategoryID = subproductID;

                string tempArtNr = String.Format("{0}{1}{2}{3}", vendorID, categoryID, productID, subproductID);
                if (_context.Articles != null)
                {
                    var dbArtID = _context.Articles.Where(x => x.ArticleNumber == tempArtNr).Select(x => x.ArticleNumber).FirstOrDefault();
                }
                article.ArticleNumber = tempArtNr;
                string tmpImgName = String.Format("{0}_{1}_{2}_{3}", vendor, category, product, subproductID);
                string newImgName = tmpImgName.Replace("&", "_");
                var serverPath = String.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", vendorID, categoryID, productID, subproductID);
                var root = _hostEnvironment.WebRootPath;
                string uploads = root + "/" + serverPath;
                Directory.CreateDirectory(uploads);
                try
                {
                    if (file.Length != 0)
                    {
                        ext = Path.GetExtension(file.FileName);
                        var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                        var tmpNameTwo = tmpName.Replace("\"", "");
                        newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
                        using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
                catch
                {
                    ViewBag.NoFile = "Du måste välja en fil";
                    return RedirectToAction("Create");
                }

                Guid guidID = Guid.NewGuid();
                article.ArticleGuid = guidID.ToString();
                article.ArticleAddDate = date;
                ImageModel img = new ImageModel
                {
                    ImageDate = date,
                    ImageName = newFilename,
                    ImagePath = String.Format("{0}", serverPath),
                    ArticleGuid = guidID
                };
                _context.Images.Add(img);
                article.ArticleImgPath = String.Format("{0}{1}", serverPath, newFilename);
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
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
        public async Task<IActionResult> NewSubCategory([Bind("SubCategoryID, SubCategoryName, ISActive")] SubCategory subCategory)
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

        private SubCatViewModel _ListSubCat(SubCatViewModel sub)
        {
            sub = new SubCatViewModel
            {
                SubCatObject = new SubCategory(),
                SubCatList = _context.SubCategories.ToList().OrderBy(x => x.SubCategoryName),
            };
            return sub;
        }

    

        private bool ArticleModelExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
        }
    }
}
