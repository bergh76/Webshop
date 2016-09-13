using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Models
{
    public class CategoryModel
    {
        [Key]
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public CategoryModel Category { get; set; }
        [Display(Name = "Category", ResourceType = typeof(Resources.CategoryModel))]
        [Required(ErrorMessage = "ErrorCategory")]
        public string CategoryName { get; set; }
        [Display(Name = "Active", ResourceType = typeof(Resources.CategoryModel))]
        public bool ISActive { get; set; }
    }
}