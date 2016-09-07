using System.ComponentModel.DataAnnotations;

namespace Webshop.Models
{
    public class SubCategory
    {
        public int ID { get; set; }
        public string SubCategoryName { get; set; }
        public bool ISActive { get; set; }
    }
}