using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Webshop.Interfaces;
using Webshop.Models;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        //private WebShopRepository _context;

        //public HomeController(WebShopRepository contect)
        //{
        //    _context = contect;
        //    //_datetime = datetime;
        //}
        public IActionResult Index(Product art)
        {
            IEnumerable<Product> prodlist = new List<Product>();
            return View(prodlist);

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
