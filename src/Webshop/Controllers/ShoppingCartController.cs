using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Webshop.ViewModels;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(WebShopRepository context, ILogger<ShoppingCartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public WebShopRepository _context { get; }

        //
        // GET: /ShoppingCart/
        public async Task<IActionResult> Index()
        {
            var cart = ShoppingCart.GetCart(_context, HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = await cart.GetCartItems(),
                CartTotal = await cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /ShoppingCart/AddToCart/5

        public async Task<IActionResult> AddToCart(int id, CancellationToken requestAborted)
        {
            // Retrieve the album from the database
            var addedArticle = await _context.Articles
                .SingleAsync(article => article.ArticleId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(_context, HttpContext);

            await cart.AddToCart(addedArticle);

            await _context.SaveChangesAsync(requestAborted);
            _logger.LogInformation("Article {0} was added to the cart.", addedArticle.ArticleId);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(
            int id,
            CancellationToken requestAborted)
        {
            // Retrieve the current user's shopping cart
            var cart = ShoppingCart.GetCart(_context, HttpContext);

            // Get the name of the album to display confirmation
            var cartItem = await _context.CartItems
                .Where(item => item.CartItemId == id)
                .Include(c => c.ArticleId)
                .SingleOrDefaultAsync();

            string message;
            int itemCount;
            if (cartItem != null)
            {
                // Remove from cart
                itemCount = cart.RemoveFromCart(id);

                await _context.SaveChangesAsync(requestAborted);

                string removed = (itemCount > 0) ? " 1 copy of " : string.Empty;
                message = removed + cartItem.ArticleTranslation.ArticleName + " has been removed from your shopping cart.";
            }
            else
            {
                itemCount = 0;
                message = "Could not find this item, nothing has been removed from your shopping cart.";
            }

            // Display the confirmation message

            var results = new ShoppingCartRemoveViewModel
            {
                Message = message,
                CartTotal = await cart.GetTotal(),
                CartCount = await cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            _logger.LogInformation("Album {id} was removed from a cart.", id);

            return Json(results);
        }
    }
}