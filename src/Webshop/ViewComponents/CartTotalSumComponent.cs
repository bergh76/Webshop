using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            ShoppingCart cart =  ShoppingCart.GetCart(_context, HttpContext);
            cart._sum = await cart.GetTotal();
            cart._items = await cart.GetCount();
            return View(cart);
        }
    }
}
