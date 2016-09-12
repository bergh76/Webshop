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
        public Articles Article { get; set; }
        public ICollection CartList { get; set; }
    }
}
