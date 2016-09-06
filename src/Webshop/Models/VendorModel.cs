using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webshop.Models

{
    public class VendorModel
    {
        [Key]
        public int ID { get; set; }
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string VendorWebPage { get; set; }
        public bool ISActive { get; set; }
    }
}