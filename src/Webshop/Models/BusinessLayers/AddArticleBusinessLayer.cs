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

        internal void AddArticle(Articles article, ArticleTranslation artTranslate,  WebShopRepository _context, IHostingEnvironment _hostEnvironment, IStringLocalizer<ArticleController> _localizer, IFormFile file, IFormCollection form)
        {
            var date = DateTime.Now.ToLocalTime();
            int vendorID = Convert.ToInt32(form["VendorID"]);
            string vendor = _context.Vendors.Where(x => x.VendorID == vendorID).Select(x => x.VendorName).FirstOrDefault();
            article.VendorId = Convert.ToInt32(vendorID);

            int categoryID = Convert.ToInt32(form["CategoryID"]);
            string category = _context.Categories.Where(x => x.CategoryID == categoryID).Select(x => x.CategoryName).FirstOrDefault();
            article.CategoryId = categoryID;

            string productID = form["ProductID"];
            string product = _context.Products.Where(x => x.ProductID == productID).Select(x => x.ProductName).FirstOrDefault();
            article.ProductId = productID;

            int subproductID = Convert.ToInt32(form["SubCategoryID"]);
            string subproduct = _context.SubCategories.Where(x => x.SubCategoryID == subproductID).Select(x => x.SubCategoryName).FirstOrDefault();
            article.SubCategoryId = subproductID;

            string tempArtNr = String.Format("{0}{1}{2}{3}", vendorID, categoryID, productID, subproductID);
            article.ArticleNumber = tempArtNr;
            Guid guidID = CreatGuid();
            article.ArticleGuid = guidID;
            article.ArticleAddDate = date;

            var image = IsImage(file); // check if file is a image
            if (image == true)
            {
                string tmpImgName = String.Format("{0}_{1}_{2}_{3}", vendor, category, product, subproductID);
                string newImgName = tmpImgName.Replace("&", "_");
                var serverPath = String.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", vendorID, categoryID, productID, subproductID);
                var root = _hostEnvironment.WebRootPath;
                string uploads = root + "/" + serverPath;
                Directory.CreateDirectory(uploads);

                string ext = Path.GetExtension(file.FileName);
                var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                var tmpNameTwo = tmpName.Replace("\"", "");
                string newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
                var fPath = Path.Combine(uploads, newFilename); // gets the path and filename for ifexists
                //var fileExists = File.Exists(fPath) || File.Exists(Path.Combine(Directory.GetParent(Path.GetDirectoryName(fPath)).FullName, Path.GetFileName(fPath))); // check if file exists in dir
                if (File.Exists(fPath) || File.Exists(Path.Combine(Directory.GetParent(Path.GetDirectoryName(fPath)).FullName, Path.GetFileName(fPath))) == true)
                {
                    ext = Path.GetExtension(file.FileName); // get file extention
                    tmpName = form["ArticleName"] + "_" + tempArtNr;
                    tmpNameTwo = tmpName.Replace("\"", "");
                    int countFInDir = Directory.GetFiles(uploads, "*", SearchOption.TopDirectoryOnly).Length; // count existing files in topdirectory ie. uploads
                    int addCount = countFInDir + 1; // increment filecount 
                    string dash = "_";
                    string tmpNameThree = string.Format("{0}{1}{2}", tmpNameTwo, dash, addCount);
                    string newFnameExists = tmpNameThree.Replace(" ", "_") + ext.ToString();
                    using (var fileStream = new FileStream(Path.Combine(uploads, newFnameExists), FileMode.Create))
                    {
                        file.CopyToAsync(fileStream);
                    }
                    ImageModel imgExists = new ImageModel
                    {
                        ImageDate = date,
                        ImageName = newFnameExists,
                        ImagePath = String.Format("{0}", serverPath),
                        ArticleGuid = guidID
                    };
                    _context.Images.Add(imgExists);
                    _context.SaveChangesAsync();
                }
                else
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
                    {
                        file.CopyToAsync(fileStream);
                    }
                    ImageModel img = new ImageModel
                    {
                        ImageDate = date,
                        ImageName = newFilename,
                        ImagePath = String.Format("{0}", serverPath),
                        ArticleGuid = guidID
                    };
                    //article.ArticleImgPath = string.Format("{0}{1}", serverPath + newFilename);
                    _context.Images.Add(img);
                    article.ImageId = _context.Images.Where(x => x.ArticleGuid == article.ArticleGuid).Select(x => x.ImageId).FirstOrDefault();

                }

            }
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