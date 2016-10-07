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
using Webshop.ViewModels;

namespace Webshop.Models.BusinessLayers
{
    public class ArticleBusinessLayer
    {
        private static readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" }; // for image checking
        private readonly WebShopRepository _context;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IStringLocalizer<ArticleBusinessLayer> _localizer;
        private readonly ILogger<ArticleBusinessLayer> _logger;
        private IFormFile _file;
        private IFormCollection _form;
        private static DateTime _datetime = DateTime.Now;
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

        public ArticleBusinessLayer(IHostingEnvironment hostEnvironment, IStringLocalizer<ArticleBusinessLayer> localizer, WebShopRepository context, IFormFile file, IFormCollection form, ILogger<ArticleBusinessLayer> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _localizer = localizer;
            _logger = logger;
            _file = file;
            _form = form;
        }

        // method to start AddArticles from crontroller
        internal async Task AddArticle(IFormFile file, IFormCollection form, WebShopRepository context, Articles article,ArticleTranslation artTranslate, IHostingEnvironment hostEnvironment, int VendorID, int ProductID, int CategoryID,int SubCategoryID)
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

            article.ArticleAddDate = _datetime;
            artTranslate.ISTranslated = false;
            context.Articles.Add(article);
            await context.SaveChangesAsync();
            var imgId = context.Articles.Select(x => x.ArticleId).LastOrDefault();
            _articleId = imgId;

            await context.SaveChangesAsync();

            await SetImagePathAndDirectoryForImageUpload(_hostEnvironment, context, article, file, form, _vendor, _category, _product, _subproduct);
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

        private void GetSubCategory(WebShopRepository context, Articles article, int SubCategoryID)
        {
            _subproductId = SubCategoryID;
            _subproduct = context.SubCategories.Where(x => x.SubCategoryID == _subproductId).Select(n => n.SubCategoryName).FirstOrDefault();
            article.SubCategoryId = _subproductId;
        }

        private void GetProduct(WebShopRepository context, Articles article, int ProductID)
        {
            _productId = ProductID;
            _product = context.Products.Where(x => x.ProductID == _productId).Select(n => n.ProductName).FirstOrDefault();
            article.ProductId = _productId;
        }

        private void GetCategory(WebShopRepository context, Articles article, int CategoryID)
        {
            _categoryId = CategoryID;
            _category = context.Categories.Where(x => x.CategoryID == _categoryId).Select(n => n.CategoryName).FirstOrDefault();
            article.CategoryId = _categoryId;
        }

        private void GetVendor(WebShopRepository context, Articles article, int VendorID)
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

        public async Task SetImagePathAndDirectoryForImageUpload(IHostingEnvironment hostEnvironment,  WebShopRepository context, Articles article, IFormFile file, IFormCollection form, string _vendor, string _category, string _product, string _subproduct)
        {
            if (IsImage(file)==true)
            {
                var serverPath = string.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", _vendorId, _categoryId, _productId, _subproductId);//creates serverpath for images
                var uploads = _root + "/" + serverPath;
                Directory.CreateDirectory(uploads); //creates directory if not exists else use allready created

                string ext = Path.GetExtension(file.FileName);
                var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                var tmpNameTwo = tmpName.Replace("\"", "");
                string newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
                var fPath = Path.Combine(uploads, newFilename); // gets the path and filename for ifexists
                if (File.Exists(fPath) || File.Exists(Path.Combine(Directory.GetParent(Path.GetDirectoryName(fPath)).FullName, Path.GetFileName(fPath))) == true)
                {
                    string newFnameExists;

                    SetFileNameIfFileExists(file, form, uploads, out ext, out tmpName, out tmpNameTwo, out newFnameExists);

                    await IfFileExistsUpload(context, file, serverPath, uploads, newFnameExists);

                }
                else
                {
                    await FileUpload(context, file, serverPath, uploads, newFilename);
                }
            }
        }

        private void SetFileNameIfFileExists(IFormFile file, IFormCollection form, string uploads, out string ext, out string tmpName, out string tmpNameTwo, out string newFnameExists)
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

        private async Task FileUpload(WebShopRepository context, IFormFile file, string serverPath, string uploads, string newFilename)
        {
            using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            ImageModel imgExists = new ImageModel
            {
                ImageDate = _datetime,
                ImageName = newFilename,
                ImagePath = String.Format("{0}", serverPath),
                ArtikelId = _articleId
            };
            context.Images.Add(imgExists);
            await context.SaveChangesAsync();
        }

        private async Task IfFileExistsUpload(WebShopRepository context, IFormFile file, string serverPath, string uploads, string newFnameExists)
        {
            using (var fileStream = new FileStream(Path.Combine(uploads, newFnameExists), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            ImageModel imgExists = new ImageModel
            {
                ImageDate = _datetime,
                ImageName = newFnameExists,
                ImagePath = String.Format("{0}", serverPath),
                ArtikelId = _articleId
            };
            context.Images.Add(imgExists);
            await context.SaveChangesAsync();
        }

        internal async Task EditArticle(Articles article, ArticleTranslation artTrans, WebShopRepository context, ImageModel img, IHostingEnvironment hostEnvironment, int id, IFormFile file, IFormCollection form)
        {
            var image = IsImage(file);
            if (image == true)
            {
                _vendorId = article.VendorId;
                _categoryId = article.CategoryId;
                _productId = article.ProductId;
                _subproductId = article.SubCategoryId;
                article.ArticleAddDate =_datetime;
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
                
                img.ImageDate = _datetime;
                img.ImageName = newFilename;
                img.ImagePath = String.Format("{0}", serverPath);
                img.ArtikelId = id;
                context.Entry(img).State = EntityState.Modified;
                context.SaveChanges();
                article.ImageId = context.Images.Where(x => x.ArtikelId == id).Select(x => x.ImageId).FirstOrDefault();
                context.Update(article);
                context.Update(artTrans);
                await context.SaveChangesAsync();
            }
            context.Entry(article).State = EntityState.Modified;
            context.Entry(artTrans).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        internal async Task Translate(int id, WebShopRepository context,ArticlesViewModel artView, ArticleTranslation artTrans,string text, string one, string two, string three, string four)
        {
            //AddTranslation(id, context, artTrans, text, one, two, three, four);
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
            context.Add(trans);
            artTrans.ISTranslated = context.ArticleTranslations.Any(x => x.ArticleId == id && x.LangCode == "sv") == true;
            context.Entry(artTrans).State = EntityState.Modified;
            await context.SaveChangesAsync();


        }

        //private static void AddTranslation(int id, WebShopRepository context, ArticleTranslation artTrans, string text, string one, string two, string three, string four)
        //{
        //    ArticleTranslation trans = new ArticleTranslation
        //    {
        //        ArticleId = id,
        //        ArticleName = artTrans.ArticleName,
        //        ArticleShortText = text,
        //        ArticleFeaturesOne = one,
        //        ArticleFeaturesTwo = two,
        //        ArticleFeaturesThree = three,
        //        ArticleFeaturesFour = four,
        //        ISTranslated = true,
        //        LangCode = "en",
        //    };
        //    context.Add(trans);
        //}

        //internal async Task DeleteArticle(int id)
        //{
        //    var article = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleId == id);
        //    var artTrans = await _context.ArticleTranslations.Where(d => d.ArticleId == id).ToListAsync();
        //    foreach (ArticleTranslation item in artTrans)
        //    {
        //        _context.ArticleTranslations.Remove(item);
        //    }
        //    _context.Articles.Remove(article);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (WebException ex)
        //    {
        //        var webException = ex.InnerException as WebException;
        //        if (webException != null)
        //        {
        //            // Here you can check for timeouts, and other connection related errors.
        //            // webException.Response could contain the response object.
        //            throw new WebException(webException.Message);
        //        }
        //        else
        //        {
        //            // In case there wasn't a WebException where you could get the response
        //            var faultResponse = (string)ex.Data["Respone"];
        //            throw new Exception(faultResponse);
        //        }
        //    }
        //}

        private bool ArticleModelExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}
