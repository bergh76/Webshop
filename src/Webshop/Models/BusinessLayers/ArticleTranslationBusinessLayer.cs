using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Webshop.ViewModels;

namespace Webshop.Models.BusinessLayers
{
    public class ArticleTranslationBusinessLayer
    {



        private static readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" };

        private readonly WebShopRepository _context;
        private readonly IHostingEnvironment _hostEnvironment;
        private IFormCollection _form;

        IEnumerable<ArticlesViewModel> vModel { get; set; }

        public ArticleTranslationBusinessLayer()
        {
        }

        public ArticleTranslationBusinessLayer(WebShopRepository context,IFormCollection form)
        {
            _context = context;
            _form = form;
        }

        internal void TranslateArticleData(ArticleTranslation artTranslate, Articles article, WebShopRepository _context, IFormCollection form)
        {

            //var date = DateTime.Now.ToLocalTime();
            //_vendorID = article.VendorId;
            //_categoryID = article.CategoryId;
            //_productID = article.ProductId;
            //_subproductID = article.SubCategoryId;


            //context.SaveChanges();
            //article.ImageId = context.Images.Where(x => x.ArticleGuid == article.ArticleGuid).Select(x => x.ImageId).FirstOrDefault();
        }
    }
}