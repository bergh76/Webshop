using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.Services;
using Webshop.ViewModels;

namespace Webshop.Components
{
    [ViewComponent(Name = "CheckOutCart")]
    public class CheckOutCartComponent : ViewComponent
    {
        private string _iso;
        private decimal _curr;
        public CheckOutCartComponent(WebShopRepository context)
        {
            _iso = new RegionInfo(CultureInfo.CurrentUICulture.Name).ISOCurrencySymbol;
            _curr = FixerIO.GetUDSToRate(_iso);
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
