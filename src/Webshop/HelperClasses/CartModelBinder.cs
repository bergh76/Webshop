using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Webshop.Models;
using Webshop.Models.BusinessLayers;

namespace Webshop.HelperClasses
{
    public class CartModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            if (bindingContext.ModelType == typeof(ShoppingCart))
            {
                var repo = bindingContext.HttpContext.RequestServices.GetService(typeof(Webshop.Models.WebShopRepository)) as Webshop.Models.WebShopRepository;

                //bool success = false;
                ShoppingCart cart = null;


                //Kolla om det kommit in en cookie med namn cart?
                if (bindingContext.HttpContext.Request.Cookies["cart"] != null)
                {
                    var value = bindingContext.HttpContext.Request.Cookies["cart"];
                    var safevalue = HtmlEncoder.Default.Encode(value);
                    cart = new ShoppingCart(repo, safevalue);
                }
                else
                {
                    //Första besöket eller inga cookies påslagna.
                    cart = new ShoppingCart(repo, "");
                }
                bindingContext.HttpContext.Response.Cookies.Append(
                    "cart",
                    cart._shoppingCartId,
                    new Microsoft.AspNetCore.Http.CookieOptions()
                    {
                        Expires = DateTime.Now.AddMonths(1),
                    }
                );

                bindingContext.Result = ModelBindingResult.Success(cart);
                return Task.CompletedTask;
            }
            // If we haven't handled it
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.FromResult<ModelBindingResult>(bindingContext.Result);
        }
    }
}
