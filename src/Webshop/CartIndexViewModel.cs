using Webshop.Models;

namespace Webshop.Controllers
{
    internal class CartIndexViewModel
    {
        public CartItem CartItem { get; set; }
        public object ISOCS { get; set; }
        public string ReturnUrl { get; set; }
    }
}