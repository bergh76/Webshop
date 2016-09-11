using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Interfaces;
using Webshop.Models;
using Webshop.ViewModels;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        private WebShopRepository _context;

        public HomeController(WebShopRepository context)
        {
            _context = context;
            //_datetime = datetime;
        }

        //public HomeController()
        //{
        //}

        //public HomeController()
        //{
        //}

        public async Task<IActionResult> Index()
        {
            var webShopRepository = _context.Articles.Include(a => a.Category).Include(a => a.Product).Include(a => a.SubCategory).Include(a => a.Vendor);
            var isCampaign = await webShopRepository.Where(x => x.ISCampaign == true).ToListAsync();
            var outCampaign = isCampaign.Count();
            if (outCampaign > 0)
            {                
                return View(isCampaign.ToList());
            }
            //if(searchOut.Count() == 0)
            //{
            //    ViewBag.NoHit = "Din sökning gav inga resultat";
            //    return View(webShopRepository.ToListAsync());
            //}
            //if (string.IsNullOrEmpty(search))
            //{
            //    //ViewBag.NoHit = "Din sökning gav inga resultat";
            //    return View(webShopRepository.ToListAsync());
            //}
            return View(await webShopRepository.ToListAsync());
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
