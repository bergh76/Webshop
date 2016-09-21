using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models.BusinessLayers
{
    public partial class ShoppingCart
    {
        private readonly WebShopRepository _context;
        private readonly string _shoppingCartId;

        private ShoppingCart(WebShopRepository db, string id)
        {
            _context = db;
            _shoppingCartId = id;
        }

        public static ShoppingCart GetCart(WebShopRepository db, HttpContext context)
            => GetCart(db, GetCartId(context));

        public static ShoppingCart GetCart(WebShopRepository db, string cartId)
            => new ShoppingCart(db, cartId);

        public async Task AddToCart(Articles article, ArticleTranslation artT)
        {
            // Get the matching cart and album instances
            var cartItem = await _context.CartItems.SingleOrDefaultAsync(
                c => c.CartId == _shoppingCartId
                && c.ArticleId == article.ArticleId
                && c.ArticleId == artT.ArticleId);


            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new CartItem
                {
                    ArticleId = article.ArticleId,
                    ArticleName = artT.ArticleName,
                    CartId = _shoppingCartId,                    
                    Count = 1,
                    DateCreated = DateTime.Now
                    
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = _context.CartItems.SingleOrDefault(
                cart => cart.CartId == _shoppingCartId
                && cart.CartItemId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }
            }

            return itemCount;
        }

        public async Task EmptyCart()
        {
            var cartItems = await _context
                .CartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .ToArrayAsync();
            _context.CartItems.RemoveRange(cartItems);
        }

        public Task<List<CartItem>> GetCartItems()
        {
            return _context
                .CartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .Include(c => c.Article)
                .Include(C => C.ArticleTranslation)
                .ToListAsync();
        }

        public Task<List<string>> GetCartArticleTitles()
        {
            return _context
                .CartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .Select(c => c.ArticleName)
                .OrderBy(n => n)
                .ToListAsync();
        }

        public Task<int> GetCount()
        {
            // Get the count of each item in the cart and sum them up
            return _context
                .CartItems
                .Where(c => c.CartId == _shoppingCartId)
                .Select(c => c.Count)
                .SumAsync();
        }

        public Task<decimal> GetTotal()
        {
            // Multiply article price by count of that article to get 
            // the current price for each of those articles in the cart
            // sum all articles price totals to get the cart total

            return _context
                .CartItems
                .Include(c => c.Article)
                .Where(c => c.CartId == _shoppingCartId)
                .Select(c => c.Article.ArticlePrice * c.Count)
                .SumAsync();
        }

        public async Task<int> CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = await GetCartItems();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                //var album = _db.Albums.Find(item.AlbumId);
                var article = await _context.Articles.SingleAsync(a => a.ArticleId == item.ArticleId);

                var orderDetail = new OrderDetail
                {
                    ArticleId = item.ArticleId,
                    //Article = item.ArticleName,

                    OrderId = order.OrderId,
                    UnitPrice = article.ArticlePrice,
                    Quantity = item.Count,
                };

                // Set the order total of the shopping cart
                orderTotal += (item.Count * article.ArticlePrice);

                _context.OrderDetails.Add(orderDetail);
            }

            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Empty the shopping cart
            await EmptyCart();

            // Return the OrderId as the confirmation number
            return order.OrderId;
        }

        // We're using HttpContextBase to allow access to sessions.
        private static string GetCartId(HttpContext context)
        {
            var cartId = context.Session.GetString("Session");
            //var isCheckout = context.Session.IsAvailable;
            if (cartId == null)
            {
                //A GUID to hold the cartId. 
                cartId = Guid.NewGuid().ToString();

                // Send cart Id as a cookie to the client.
                context.Session.SetString("Session", cartId);
            }
            return cartId;
        }
    }
}
