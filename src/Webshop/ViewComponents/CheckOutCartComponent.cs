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
    [ViewComponent(Name = "CheckOutCart")]
    public class CheckOutCartComponent : ViewComponent
    {
        public CheckOutCartComponent(WebShopRepository context)
        {
            _context = context;
        }

        private WebShopRepository _context { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ShoppingCart cart = ShoppingCart.GetCart(_context, HttpContext);
            cart._cartItems = await cart.GetCartItems();
            cart._artList = await cart.GetCartItems();
            var sum = await cart.GetTotal();
            cart._sum = Math.Round(sum, 2);
            return View(cart);
        }
    }
}
