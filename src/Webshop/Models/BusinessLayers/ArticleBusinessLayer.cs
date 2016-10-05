using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IStringLocalizer<ArticleController> _localizer;

        private IFormFile _file;
        private IFormCollection _form;

        DateTime date = DateTime.Now.ToLocalTime();
        private static Guid guidID;
        private static string root;
        string tempArtNr;
        private int _vendorID;
        private int _categoryID;
        private int _productID;
        private int _subproductID;
        private string _vendor;
        private string _category;
        private string _product;
        private string _subproduct;

        public ArticleBusinessLayer() { }

        public ArticleBusinessLayer(IHostingEnvironment hostEnvironment, IStringLocalizer<ArticleController> localizer, WebShopRepository context, IFormFile file, IFormCollection form)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _localizer = localizer;
            _file = file;
            _form = form;
        }

        internal async Task AddArticle(IFormFile file, IFormCollection form, WebShopRepository context, Articles article,ArticleTranslation artTranslate, IHostingEnvironment hostEnvironment, int VendorID, int ProductID, int CategoryID,int SubCategoryID)
        {
            var rootHost = RootHost(hostEnvironment);

            artTranslate.LangCode = "sv";
            // get vendor
            GetVendor(context, article, VendorID);
            // get category
            GetCategory(context, article, CategoryID);
            // get product
            GetProduct(context, article, ProductID);
            // get subcategory
            GetSubCategory(context, article, SubCategoryID);
            // set tempfilename for image upload
            SetTemFileName(article, artTranslate);
            // creates a gui for image<>article relation
            Guid guidID = CreatGuid();
            article.ArticleGuid = guidID;
            article.ArticleAddDate = date;
            artTranslate.ISTranslated = false;

            await Image(_hostEnvironment, context, article, file, form, _vendor, _category, _product, _subproduct);
            context.Articles.Add(article);
            article.ImageId = context.Images.Where(x => x.ArticleGuid == article.ArticleGuid).Select(x => x.ImageId).FirstOrDefault();

            await context.SaveChangesAsync();
            artTranslate.ArticleId = context.Articles.Where(x => x.ArticleGuid == guidID).Select(x => x.ArticleId).FirstOrDefault();
            context.ArticleTranslations.Add(artTranslate);
            await context.SaveChangesAsync();
        }

        private void SetTemFileName(Articles article, ArticleTranslation artTranslate)
        {
            tempArtNr = String.Format("{0}{1}{2}{3}", _vendorID, _categoryID, _productID, _subproductID);
            article.ArticleNumber = tempArtNr;
            artTranslate.ArticleNumber = tempArtNr;
        }

        private void GetSubCategory(WebShopRepository context, Articles article, int SubCategoryID)
        {
            _subproductID = SubCategoryID;
            _subproduct = context.SubCategories.Where(x => x.SubCategoryID == _subproductID).Select(n => n.SubCategoryName).FirstOrDefault();
            article.SubCategoryId = _subproductID;
        }

        private void GetProduct(WebShopRepository context, Articles article, int ProductID)
        {
            _productID = ProductID;
            _product = context.Products.Where(x => x.ProductID == _productID).Select(n => n.ProductName).FirstOrDefault();
            article.ProductId = _productID;
        }

        private void GetCategory(WebShopRepository context, Articles article, int CategoryID)
        {
            _categoryID = CategoryID;
            _category = context.Categories.Where(x => x.CategoryID == _categoryID).Select(n => n.CategoryName).FirstOrDefault();
            article.CategoryId = _categoryID;
        }

        private void GetVendor(WebShopRepository context, Articles article, int VendorID)
        {
            _vendorID = VendorID;
            _vendor = context.Vendors.Where(x => x.VendorID == _vendorID).Select(n => n.VendorName).FirstOrDefault();
            article.VendorId = Convert.ToInt32(_vendorID);
        }

        public async Task Image(IHostingEnvironment hostEnvironment,  WebShopRepository context, Articles article, IFormFile file, IFormCollection form, string _vendor, string _category, string _product, string _subproduct)
        {
            if (IsImage(file)==true)
            {
                var serverPath = string.Format("images/imageupload/v/{0}/c/{1}/p/{2}/s/{3}/", _vendorID, _categoryID, _productID, _subproductID);//creates serverpath for images
                var uploads = root + "/" + serverPath;
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

        private async Task IfFileExistsUpload(WebShopRepository context, IFormFile file, string serverPath, string uploads, string newFnameExists)
        {
            using (var fileStream = new FileStream(Path.Combine(uploads, newFnameExists), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            ImageModel imgExists = new ImageModel
            {
                ImageDate = date,
                ImageName = newFnameExists,
                ImagePath = String.Format("{0}", serverPath),
                ArticleGuid = guidID
            };
            context.Images.Add(imgExists);
            await context.SaveChangesAsync();
        }

        private async Task FileUpload(WebShopRepository context, IFormFile file, string serverPath, string uploads, string newFilename)
        {
            using (var fileStream = new FileStream(Path.Combine(uploads, newFilename), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            ImageModel img = new ImageModel
            {
                ImageDate = date,
                ImageName = newFilename,
                ImagePath = String.Format("{0}", serverPath),
                ArticleGuid = guidID
            };
            context.Images.Add(img);
            await context.SaveChangesAsync();
        }

        internal async Task EditArticle(Articles article, ArticleTranslation artTrans, WebShopRepository context, IHostingEnvironment hostEnvironment, int id, IFormFile file, IFormCollection form)
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
                string ext = Path.GetExtension(file.FileName);
                var tmpName = form["ArticleName"] + "_" + tempArtNr; //date.ToString("_yyyymmddmmhhss");
                var tmpNameTwo = tmpName.Replace("\"", "");
                string newFilename = tmpNameTwo.Replace(" ", "_") + ext.ToString();
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
                context.Update(article);
                context.Update(artTrans);
                await context.SaveChangesAsync();
            }
            context.Entry(article).State = EntityState.Modified;
            context.Entry(artTrans).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        private Guid CreatGuid()
        {
            guidID = Guid.NewGuid();
            return guidID;
        }

        private static string RootHost(IHostingEnvironment host)
        {
            return root = host.WebRootPath;
        }

        private static bool IsImage(IFormFile file)
        {   
            // checks if file is image
            if (file == null) return false;
            return file.ContentType.Contains("image") ||
                _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        internal async Task Translate(int id, WebShopRepository context,ArticlesViewModel artView, ArticleTranslation artTrans,string text, string one, string two, string three, string four)
        {
            AddTranslation(id, context, artTrans, text, one, two, three, four);

            // TODO: update isTranslated for se-SV
            var isTranslated = context.ArticleTranslations
                .Where(x => x.ArticleId == id)
                .Select(x => x.ISTranslated).FirstOrDefault();
            isTranslated = true;
            artTrans.ISTranslated = isTranslated;

            await context.SaveChangesAsync();
        }

        private static void AddTranslation(int id, WebShopRepository context, ArticleTranslation artTrans, string text, string one, string two, string three, string four)
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
                ArticleNumber = artTrans.ArticleNumber
            };
            context.Add(trans);
        }
    }
}
