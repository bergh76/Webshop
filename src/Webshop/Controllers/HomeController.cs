using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Webshop.Interfaces;
using Webshop.Models;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
            //_datetime = datetime;
        }
        public IActionResult Index(ArticleModel art)
        {
            IEnumerable<ArticleModel> artList = new List<ArticleModel>();
            //var isCampaign = _context.Article.Where(x => x.ISCampaign == true).ToListAsync();
            //var outCampaign = isCampaign.Count();
            //if (outCampaign > 0)
            //{
            //    artList = isCampaign.ToList();
            //    return View(artList);
            //}
            //var cart = new ShoppingCart();
            //cart.ItemsList();
            return View(artList);

        }

        public IActionResult About(int id, string name, string lang)
        {
            ViewData["Message"] = lang +" "+  id + " " + name;

            return View();
        }

        public IActionResult Contact([FromServices]IDateTime _datetime)
        {
            ContactViewModel vmodel = new ContactViewModel();
            vmodel.CurrentDateAndTime = _datetime.Now.ToString();
            vmodel.id = 0;
            List<string> list = new List<string>();
            list.Add("Andreas");
            list.Add("Tjorven");
            vmodel.Names = list;

            return View(vmodel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
