using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.ViewModels;

namespace Webshop.Components
{
    [ViewComponent(Name = "CartTotalSum")]
    public class CartTotalSumComponent: ViewComponent
    {
        public CartTotalSumComponent(WebShopRepository context)
        {
            _context = context;
        }
        private WebShopRepository _context { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = ShoppingCart.GetCart(_context, HttpContext);
            var sum = await cart.GetTotal();
            var items = await cart.GetCount();
            ViewBag.Sum = sum;
            ViewBag.Items = items;
            return View();
        }
    }
}
