using System.ComponentModel.DataAnnotations;

namespace Webshop.Models
{
    public class SubCategory
    {
        [Key]
        public int ID { get; set; }
        public int SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }
        public bool ISActive { get; set; }
    }
}