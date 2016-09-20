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
    [ViewComponent(Name = "CartSummary")]
    public class CartSummaryComponent : ViewComponent
    {
        public CartSummaryComponent(WebShopRepository context)
        {
            _context = context;
        }

        private WebShopRepository _context { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = ShoppingCart.GetCart(_context, HttpContext);

            var cartItems = await cart.GetCartArticleTitles();

            ViewBag.CartCount = cartItems.Count;
            ViewBag.CartSummary = string.Join("\n", cartItems.Distinct());
            ViewBag.List = cartItems.ToList();
            var sum = await cart.GetTotal();
            ViewBag.Sum = sum.ToString();
            return View();
        }
    }
}
