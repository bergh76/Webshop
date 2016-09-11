

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop.ViewComponents
{
    public class BreadCrumTrackerViewComponent : ViewComponent
    {
        private WebShopRepository _context;

        public BreadCrumTrackerViewComponent(WebShopRepository context)
        {
            _context = context;
        }
        public async Task <IViewComponentResult> InvokeAsync()
        {
            BreadCrumTracker bc = new BreadCrumTracker(_context);
            await bc.GetTracker();
            return View(bc);
        }
    }
}