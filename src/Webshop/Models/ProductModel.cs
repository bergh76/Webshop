using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models
{ 
    public class ProductModel
    {
        [Key]
        public int ID { get; set; }
        public string ProductID { get; set; }
        [Display(Name = "Product", ResourceType = typeof(Resources.ProductModel))]
        [Required(ErrorMessage = "ProductError")]
        public string ProductName { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Resources.ProductModel))]
        public bool ISActive { get; set; }
        public ICollection<ProductModel> Products { get; set; }
    }
}
