using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Webshop.Controllers;
using Webshop.Interfaces;
using Webshop.ViewModels;

namespace Webshop.Models.BusinessLayers
{
    public class ArticleBusinessLayer
    {
        private static readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" }; // for image checking
        private readonly WebShopRepository _context;
        private readonly IDateTime _datetime;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IStringLocalizer<ArticleBusinessLayer> _localizer;
        private readonly ILogger<ArticleBusinessLayer> _logger;
        private IFormFile _file;
        private IFormCollection _form;
        private static int _articleId;
        private static string _root;
        private string tempArtNr;
        private int _vendorId;
        private int _categoryId;
        private int _productId;
        private int _subproductId;
        private string _vendor;
        private string _category;
        private string _product;
        private string _subproduct;

        public ArticleBusinessLayer() { }

        public ArticleBusinessLayer(WebShopRepository context,IDateTime datetime, IHostingEnvironment hostEnvironment, IStringLocalizer<ArticleBusinessLayer> localizer, IFormFile file, IFormCollection form, ILogger<ArticleBusinessLayer> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _localizer = localizer;
            _logger = logger;
            _file = file;
            _form = form;
            _datetime = datetime;
        }

        // method to start AddArticles from crontroller
        internal async Task AddArticle(WebShopRepository context, Articles article, ArticleTranslation artTranslate, IDateTime datetime ,IFormFile file, IFormCollection form, IHostingEnvironment hostEnvironment, int VendorID, int ProductID, int CategoryID,int SubCategoryID)
        {
            var rootHost = RootHost(hostEnvironment);

            artTranslate.LangCode = "sv";
            // get vendor and set value to articleTranslate
            GetVendor(context, article, VendorID);
            // get category set value to articleTranslate
            GetCategory(context, article, CategoryID);
            // get product set value to articleTranslate
            GetProduct(context, article, ProductID);
            // get subcategory set value to articleTranslate
            GetSubCategory(context, article, SubCategoryID);
            // set tempfilename for image upload
            SetTempFileName(article);
            // creates a gui for image<>article relation

            article.ArticleAddDate = datetime.Now;
            artTranslate.ISTranslated = false;
            context.Articles.Add(article);
            await context.SaveChangesAsync();
            var imgId = context.Articles.Select(x => x.ArticleId).LastOrDefault();
            _articleId = imgId;

            await context.SaveChangesAsync();

            await SetImagePathAndDirectoryForImageUpload(context,datetime,_hostEnvironment, article, file, form, _vendor, _category, _product, _subproduct);
            artTranslate.ArticleId = _articleId;
            context.ArticleTranslations.Add(artTranslate);

            article.ImageId = context.Images.Where(x => x.ArtikelId == article.ArticleId).Select(x => x.ImageId).LastOrDefault();
            await context.SaveChangesAsync();

        }

        //method to get webroot for imageupload
        private static string RootHost(IHostingEnvironment host)
        {
            return _root = host.WebRootPath;
        }

        private void GetSubCategory(WebShopRepository context,Articles article, int SubCategoryID)
        {
            _subproductId = SubCategoryID;
            _subproduct = context.SubCategories.Where(x => x.SubCategoryID == _subproductId).Select(n => n.SubCategoryName).FirstOrDefault();
            article.SubCategoryId = _subproductId;
        }

        private void GetProduct([FromServices] WebShopRepository context,Articles article, int ProductID)
        {
            _productId = ProductID;
            _product = context.Products.Where(x => x.ProductID == _productId).Select(n => n.ProductName).FirstOrDefault();
            article.ProductId = _productId;
        }

        private void GetCategory([FromServices] WebShopRepository context, Articles article, int CategoryID)
        {
            _categoryId = CategoryID;
            _category = context.Categories.Where(x => x.CategoryID == _categoryId).Select(n => n.CategoryName).FirstOrDefault();
            article.CategoryId = _categoryId;
        }

        private void GetVendor(WebShopRepository context,Articles article, int VendorID)
        {
            _vendorId = VendorID;
            _vendor = context.Vendors.Where(x => x.VendorID == _vendorId).Select(n => n.VendorName).FirstOrDefault();
            article.VendorId = Convert.ToInt32(_vendorId);
        }

        private void SetTempFileName(Articles article)
        {
            tempArtNr = String.Format("{0}{1}{2}{3}", _vendorId, _categoryId, _productId, _subproductId);
            article.ArticleNumber = tempArtNr;

        }

        // checks if file is image
        private static bool IsImage(IFormFile file)
        {   
            if (file == null) return false;
            return file.ContentType.Contains("image") ||
                _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        public async Task SetImagePathAndDirectoryForImageUpload(WebShopRepository context,IDateTime datetime,IHostingEnvironment hostEnvironment, Articles article, IFormFile file, IFormCollection form, string _vendor, string _category, string _product, string _subproduct)
        {
            if (IsImage(file)==true)
            {
                var serverPath = string.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", _vendorId, _categoryId, _productId, _subproductId);//creates serverpath for images
                var uploads = _root + "/" + serverPath;
                Directory.CreateDirectory(uploads); //creates directory if not exists else use allready created

                // filename parser
                string ext = Path.GetExtension(file.FileName);
                var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                var tmpNameTwo = tmpName.Replace("\"", "");
                string newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
                var fPath = Path.Combine(uploads, newFilename); // gets the path and filename for ifexists
                if (File.Exists(fPath) || File.Exists(Path.Combine(Directory.GetParent(Path.GetDirectoryName(fPath)).FullName, Path.GetFileName(fPath))) == true)
                {
                    string newFnameExists;

                    SetFileNameIfFileExists(datetime,file, form, uploads, out ext, out tmpName, out tmpNameTwo, out newFnameExists);

                    await IfFileExistsUpload( context,datetime, file, serverPath, uploads, newFnameExists);

                }
                else
                {
                    await FileUpload(context, datetime,file, serverPath, uploads, newFilename);
                }
            }
        }

        private void SetFileNameIfFileExists(IDateTime datetime,IFormFile file, IFormCollection form, string uploads, out string ext, out string tmpName, out string tmpNameTwo, out string newFnameExists)
        {
            ext = Path.GetExtension(file.FileName); // get file extention
            tmpName = form["ArticleName"] + "_" + tempArtNr;
            tmpNameTwo = tmpName.Replace("\"", "");
            int countFInDir = Directory.GetFiles(uploads, "*", SearchOption.TopDirectoryOnly).Length; // count existing files in topdirectory ie. uploads
            int addCount = countFInDir + 1; // increment filecount 
            string dash = "_";
            string tmpNameThree = string.Format("{0}{1}{2}", tmpNameTwo, dash, addCount);
            newFnameExists = tmpNameThree.Replace(" ", "_") + ext.ToString();
        }

        private async Task FileUpload(WebShopRepository context, IDateTime datetime, IFormFile file, string serverPath, string uploads, string newFilename)
        {
            using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            ImageModel imgExists = new ImageModel
            {
                ImageDate = datetime.Now,
                ImageName = newFilename,
                ImagePath = String.Format("{0}", serverPath),
                ArtikelId = _articleId
            };
            _context.Images.Add(imgExists);
            await _context.SaveChangesAsync();
        }

        private async Task IfFileExistsUpload(WebShopRepository context, IDateTime datetime, IFormFile file, string serverPath, string uploads, string newFnameExists)
        {
            using (var fileStream = new FileStream(Path.Combine(uploads, newFnameExists), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            ImageModel imgExists = new ImageModel
            {
                ImageDate = datetime.Now,
                ImageName = newFnameExists,
                ImagePath = String.Format("{0}", serverPath),
                ArtikelId = _articleId
            };
            _context.Images.Add(imgExists);
            await _context.SaveChangesAsync();
        }

        internal async Task EditArticle(WebShopRepository context,IDateTime datetime,Articles article, ArticleTranslation artTrans, ImageModel img, IHostingEnvironment hostEnvironment, int id, IFormFile file, IFormCollection form)
        {
            var image = IsImage(file);
            if (image == true)
            {
                _vendorId = article.VendorId;
                _categoryId = article.CategoryId;
                _productId = article.ProductId;
                _subproductId = article.SubCategoryId;
                article.ArticleAddDate =_datetime.Now;
                string tempArtNr = String.Format("{0}{1}{2}{3}", _vendorId, _categoryId, _productId, _subproductId);
                article.ArticleNumber = tempArtNr;
                var serverPath = String.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", _vendorId, _categoryId, _productId, _subproductId);
                var root = hostEnvironment.WebRootPath;
                string uploads = root + "/" + serverPath;

                Directory.CreateDirectory(uploads);
                string ext = Path.GetExtension(file.FileName);
                var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                var tmpNameTwo = tmpName.Replace("\"", "");
                string newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
                using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                
                img.ImageDate = _datetime.Now;
                img.ImageName = newFilename;
                img.ImagePath = String.Format("{0}", serverPath);
                img.ArtikelId = id;
                _context.Entry(img).State = EntityState.Modified;
                _context.SaveChanges();
                article.ImageId = _context.Images.Where(x => x.ArtikelId == id).Select(x => x.ImageId).FirstOrDefault();
                _context.Update(article);
                _context.Update(artTrans);
                await _context.SaveChangesAsync();
            }
            _context.Entry(article).State = EntityState.Modified;
            _context.Entry(artTrans).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        internal async Task Translate(int id,WebShopRepository context, ArticlesViewModel artView, ArticleTranslation artTrans,string text, string one, string two, string three, string four)
        {
            ArticleTranslation trans = new ArticleTranslation
            {
                ArticleId = id,
                ArticleName = artTrans.ArticleName,
                ArticleShortText = text,
                ArticleFeaturesOne = one,
                ArticleFeaturesTwo = two,
                ArticleFeaturesThree = three,
                ArticleFeaturesFour = four,
                ISTranslated = true,
                LangCode = "en",
            };
            _context.Add(trans);
            artTrans.ISTranslated = _context.ArticleTranslations.Any(x => x.ArticleId == id && x.LangCode == "sv") == true;
            _context.Entry(artTrans).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private bool ArticleModelExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}
