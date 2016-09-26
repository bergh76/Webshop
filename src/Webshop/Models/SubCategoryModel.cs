using System.ComponentModel.DataAnnotations;

namespace Webshop.Models
{
    public class SubCategoryModel
    {
        [Key]
        public int ID { get; set; }
        public int SubCategoryID { get; set; }
        [Display(Name = "SubCategoryType", ResourceType = typeof(Resources.SubCategoryModel))]
        [Required(ErrorMessage = "ErrorSubCategory")]
        public string SubCategoryName { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Resources.SubCategoryModel))]
        public bool ISActive { get; set; }
        public string LangCode { get; set; }

    }
}