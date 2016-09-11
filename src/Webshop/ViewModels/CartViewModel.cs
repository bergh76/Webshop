using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop.ViewModels
{
    public class CartViewModel
    {
        public ArticleModel Article { get; set; }
        public ICollection CartList { get; set; }
    }
}
