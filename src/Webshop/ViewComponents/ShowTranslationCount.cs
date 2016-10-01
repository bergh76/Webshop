using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop.ViewComponents
{
    [ViewComponent(Name = "TranslationCount")]

    public class ShowTranslationCount : ViewComponent
    {
        //public ShowTranslationCount(WebShopRepository context)
        //{
        //    _context = context;
        //}
        //private WebShopRepository _context { get; }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var obj = new TranslationData(_context);
        //    var count = await obj.CountTranslation();
        //    var names = await obj.GetNonTranslated();
        //    ViewBag.TransCount = count;
        //    ViewBag.TransNames = names.ToList();
        //    return View();
        //}
        public int _count { get; set; }
        public IEnumerable<ArticleTranslation> _names { get; set; }
        public ShowTranslationCount(WebShopRepository context)
        {
            _context = context;
        }
        private WebShopRepository _context { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            TranslationData obj = new TranslationData(_context);
            obj._count = await obj.CountTranslation();
            obj._names = await obj.GetNonTranslated();
            return View(obj);
        }
    }
}