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
        public string ProductName { get; set; }
        public bool ISActive { get; set; }
    }
}
