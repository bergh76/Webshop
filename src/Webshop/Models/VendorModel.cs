using System.ComponentModel.DataAnnotations;

namespace Webshop.Models

{
    public class VendorModel
    {
        public int ID { get; set; }
        public int VendorID { get; set; }
        [Display(Name = "VendorName", ResourceType = typeof(Resources.VendorModel))]
        [Required(ErrorMessage = "VendorError")]
        public string VendorName { get; set; }
        [Display(Name = "VendorUrl", ResourceType = typeof(Resources.VendorModel))]
        public string VendorWebPage { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Resources.VendorModel))]
        public bool ISActive { get; set; }
        public string LangCode { get; set; }


    }
}