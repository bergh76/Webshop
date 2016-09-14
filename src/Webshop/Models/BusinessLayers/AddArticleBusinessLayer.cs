using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Controllers;

namespace Webshop.Models.BusinessLayers
{
    public class AddArticleBusinessLayer
    {

        private static readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" };
        private readonly WebShopRepository _context;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IStringLocalizer<ArticleController> _localizer;

        private IFormFile _file;
        private IFormCollection _form;

        public AddArticleBusinessLayer() { }
        public AddArticleBusinessLayer(IHostingEnvironment hostEnvironment, IStringLocalizer<ArticleController> localizer, WebShopRepository context, IFormFile file, IFormCollection form)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _localizer = localizer;
            _file = file;
            _form = form;
        }


        internal void AddArticle(Articles article, WebShopRepository _context, IHostingEnvironment _hostEnvironment, IStringLocalizer<ArticleController> _localizer, IFormFile file, IFormCollection form)
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

            article.ArticleNumber = tempArtNr;
            string tmpImgName = String.Format("{0}_{1}_{2}_{3}", vendor, category, product, subproductID);
            string newImgName = tmpImgName.Replace("&", "_");
            var serverPath = String.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", vendorID, categoryID, productID, subproductID);
            var root = _hostEnvironment.WebRootPath;
            string uploads = root + "/" + serverPath;
            string newFilename = "";
            string ext = "";
            Directory.CreateDirectory(uploads);
            var image = IsImage(file);
            if (image == true)
            {
                ext = Path.GetExtension(file.FileName);
                var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                var tmpNameTwo = tmpName.Replace("\"", "");
                newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
                using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                }
            }
            Guid guidID = CreatGuid();
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
            _context.Articles.Add(article);
            _context.SaveChangesAsync();
        }

        private Guid CreatGuid()
        {            
            return Guid.NewGuid();
        }

        private static bool IsImage(IFormFile file)
        {
            if (file == null) return false;
            return file.ContentType.Contains("image") ||
                _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
        
    }
}