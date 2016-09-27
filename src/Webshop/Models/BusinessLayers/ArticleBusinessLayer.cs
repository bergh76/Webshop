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
    public class ArticleBusinessLayer
    {
        private static readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" };
        private readonly WebShopRepository _context;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IStringLocalizer<ArticleController> _localizer;

        private IFormFile _file;
        private IFormCollection _form;

        DateTime date = DateTime.Now.ToLocalTime();
        public Guid guidID;
        string tempArtNr;
        private int vendorID;
        private int categoryID;
        private int productID;
        private int subproductID;
        private int _vendorID;
        private int _categoryID;
        private string _productID;
        private int _subproductID;

        public ArticleBusinessLayer() { }

        public ArticleBusinessLayer(IHostingEnvironment hostEnvironment, IStringLocalizer<ArticleController> localizer, WebShopRepository context, IFormFile file, IFormCollection form)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _localizer = localizer;
            _file = file;
            _form = form;
        }

        internal async Task AddArticle(Articles article, ArticleTranslation artTranslate, WebShopRepository context, IFormCollection form)
        {
            string lang = "sv";
            artTranslate.LangCode = lang;

            vendorID = Convert.ToInt32(form["VendorID"]);
            article.VendorId = Convert.ToInt32(vendorID);

            categoryID = Convert.ToInt32(form["CategoryID"]);
            article.CategoryId = categoryID;

            string productID = form["ProductID"];
            article.ProductId = productID;

            subproductID = Convert.ToInt32(form["SubCategoryID"]);
            article.SubCategoryId = subproductID;

            tempArtNr = String.Format("{0}{1}{2}{3}", vendorID, categoryID, productID, subproductID);
            article.ArticleNumber = tempArtNr;
            artTranslate.ArticleNumber = tempArtNr;
            Guid guidID = CreatGuid();
            article.ArticleGuid = guidID;
            article.ArticleAddDate = date;
            artTranslate.ISTranslated = false;
            context.Articles.Add(article);
            await context.SaveChangesAsync();
            artTranslate.ArticleId = context.Articles.Where(x => x.ArticleGuid == guidID).Select(x => x.ArticleId).FirstOrDefault();
            context.ArticleTranslations.Add(artTranslate);
            await context.SaveChangesAsync();
        }
        public void Image(WebShopRepository context, Articles article, IFormFile file, IFormCollection form, IHostingEnvironment hostEnvironment)
        {
            var image = IsImage(file); // check if file is a image
            if (image == true)
            {
                var serverPath = String.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", vendorID, categoryID, productID, subproductID);//creates serverpath for images
                var root = hostEnvironment.WebRootPath;
                string uploads = root + "/" + serverPath;
                Directory.CreateDirectory(uploads); //creates directory if not exists else use allready created

                string ext = Path.GetExtension(file.FileName);
                var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                var tmpNameTwo = tmpName.Replace("\"", "");
                string newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
                var fPath = Path.Combine(uploads, newFilename); // gets the path and filename for ifexists
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
                    context.Images.Add(imgExists);
                    context.SaveChangesAsync();
                    article.ImageId = context.Images.Where(x => x.ArticleGuid == article.ArticleGuid).Select(x => x.ImageId).FirstOrDefault();

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
                    context.Images.Add(img);
                    context.SaveChanges();
                    article.ImageId = context.Images.Where(x => x.ArticleGuid == article.ArticleGuid).Select(x => x.ImageId).FirstOrDefault();
                }
            }
        }

        internal async Task EditArticle(Models.Articles article, ArticleTranslation artTrans, WebShopRepository context, IHostingEnvironment hostEnvironment, int id, string ext, string newFilename, IFormFile file, IFormCollection form)
        {
            var image = IsImage(file);
            if (image == true)
            {
                var date = DateTime.Now.ToLocalTime();
                _vendorID = article.VendorId;
                _categoryID = article.CategoryId;
                _productID = article.ProductId;
                _subproductID = article.SubCategoryId;

                string tempArtNr = String.Format("{0}{1}{2}{3}", _vendorID, _categoryID, _productID, _subproductID);
                article.ArticleNumber = tempArtNr;
                var serverPath = String.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", _vendorID, _categoryID, _productID, _subproductID);
                var root = hostEnvironment.WebRootPath;
                string uploads = root + "/" + serverPath;

                Directory.CreateDirectory(uploads);
                ext = Path.GetExtension(file.FileName);
                var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                var tmpNameTwo = tmpName.Replace("\"", "");
                newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
                using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                ImageModel img = new ImageModel
                {
                    ImageDate = date,
                    ImageName = newFilename,
                    ImagePath = String.Format("{0}", serverPath),
                    ArticleGuid = article.ArticleGuid
                };
                context.Images.Add(img);
                context.SaveChanges();
                article.ImageId = context.Images.Where(x => x.ArticleGuid == article.ArticleGuid).Select(x => x.ImageId).FirstOrDefault();
                _context.Update(article);
                _context.Update(artTrans);
                await _context.SaveChangesAsync();
            }
        }

        private Guid CreatGuid()
        {
            guidID = Guid.NewGuid();
            return guidID;
        }

        private static bool IsImage(IFormFile file)
        {
            if (file == null) return false;
            return file.ContentType.Contains("image") ||
                _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

    }
}
