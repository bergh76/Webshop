using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Webshop.Interfaces;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IDateTime _datetime;
        //public HomeController(IDateTime datetime)
        //{
        //   _datetime = datetime;
        //}
        public HomeController()
        {
            //_datetime = datetime;
        }
        public IActionResult Index()
        {
            return View();
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
