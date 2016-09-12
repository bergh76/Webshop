using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;
using Newtonsoft.Json;


namespace Webshop.BusinessLayers
{
    public class EditArticleBusiness
    {
        private static readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" };

        private readonly WebShopRepository _context;
        private readonly IHostingEnvironment _hostEnvironment;
        private string _ext;
        private string _newFilename;
        private IFormFile _file;
        private IFormCollection _form;
        private Articles _article;
        private object _message;
        private string v;
        private string c;
        private string p;
        private string s;

        private string Message { get; set; }
        public EditArticleBusiness()
        {
        }

        public EditArticleBusiness(IHostingEnvironment hostEnvironment, WebShopRepository context, string ext, string newFilename, IFormFile file, IFormCollection form, Articles article, string message)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _ext = ext;
            _newFilename = newFilename;
            _file = file;
            _form = form;
            _article = article;
            _message = message;
        }

        internal void UpdateArticleData(Articles article, WebShopRepository _context)
        {
            var date = DateTime.Now.ToLocalTime();
            int vendorID = article.VendorID;
            string vName = _context.Vendors.Where(x => x.VendorID == vendorID).Select(x => x.VendorName).FirstOrDefault();
            article.VendorID = vendorID;

            int categoryID = article.CategoryID;
            string cName = _context.Categories.Where(x => x.CategoryID == categoryID).Select(x => x.CategoryName).FirstOrDefault();

            string productID = article.ProductID;
            string pName = _context.Products.Where(x => x.ProductID == productID).Select(x => x.ProductName).FirstOrDefault();

            string subproduct = p;
            int subproductID = article.SubCategoryID;
            string subpName = _context.SubCategories.Where(x => x.SubCategoryID == subproductID).Select(x => x.SubCategoryName).FirstOrDefault();
            article.SubCategoryID = subproductID;
            _context.Update(article);
             //_context.SaveChanges();
            //return _context.SaveChanges();
        }

        internal void UpdateArticleImage(Articles article, IHostingEnvironment _hostEnvironment, WebShopRepository _context, string ext, string newFilename, IFormFile file, IFormCollection form, string message)
        {

            var date = DateTime.Now.ToLocalTime();
            int vendorID = article.VendorID;
            string vName = _context.Vendors.Where(x => x.VendorID == vendorID).Select(x => x.VendorName).FirstOrDefault();
            article.VendorID = vendorID;

            int categoryID = article.CategoryID;
            string cName = _context.Categories.Where(x => x.CategoryID == categoryID).Select(x => x.CategoryName).FirstOrDefault();

            string productID = article.ProductID;
            string pName = _context.Products.Where(x => x.ProductID == productID).Select(x => x.ProductName).FirstOrDefault();

            string subproduct = p;
            int subproductID = article.SubCategoryID;
            string subpName = _context.SubCategories.Where(x => x.SubCategoryID == subproductID).Select(x => x.SubCategoryName).FirstOrDefault();
            article.SubCategoryID = subproductID;
            var image = EditArticleBusiness.IsImage(file);
            if (image == true)
            {
                string tempArtNr = String.Format("{0}{1}{2}{3}", vendorID, categoryID, productID, subproductID);
                var dbArtID = _context.Articles.ToList().Where(x => x.ArticleNumber == tempArtNr).Select(x => x.ArticleNumber).FirstOrDefault();
                article.ArticleNumber = tempArtNr;
                string tmpImgName = String.Format("{0}_{1}_{2}_{3}", vName, cName, pName, subpName);
                string newImgName = tmpImgName.Replace("&", "_");
                var serverPath = String.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", vendorID, categoryID, productID, subproductID);
                var root = _hostEnvironment.WebRootPath;
                string uploads = root + "/" + serverPath;

                Directory.CreateDirectory(uploads);
                ext = Path.GetExtension(file.FileName);
                var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                var tmpNameTwo = tmpName.Replace("\"", "");
                newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
                if (article.ArticleImgPath == uploads + newFilename)
                {
                    File.Delete(uploads);
                    Directory.Delete(uploads);
                }
                using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                }
                article.ArticleImgPath = String.Format("{0}{1}", serverPath, newFilename);
                ImageModel img = new ImageModel
                {
                    ImageDate = date,
                    ImageName = newFilename,
                    ImagePath = String.Format("{0}", serverPath),
                    ArticleGuid = new Guid(article.ArticleGuid)
                };
                _context.Images.Add(img);
            }


            //_context.SaveChanges();
            //return GetAwaiter(article,_hostEnvironment, _context, ext, newFilename, file, form, message);
        }

        private static bool IsImage(IFormFile file)
        {
            if (file == null) return false;
            return file.ContentType.Contains("image") ||
                _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}