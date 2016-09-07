using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Models
{
    public class ProductCategory
    {
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }

        public ICollection<Product> Product { get; set; }
    }
}