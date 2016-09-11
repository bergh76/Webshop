

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop.ViewComponents
{
    [ViewComponent(Name = "BreadCrumTracker")]
    public class BreadCrumTrackerViewComponent : ViewComponent
    {
        private WebShopRepository _context;

        public BreadCrumTrackerViewComponent(WebShopRepository context)
        {
            _context = context;
        }
        public async Task <IViewComponentResult> InvokeAsync(int id)
        {
            BreadCrumTracker bc = new BreadCrumTracker(_context, id);
            await bc.GetTracker();
            return View(bc);
        }
    }
}