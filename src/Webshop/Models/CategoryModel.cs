using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Models
{
    public class CategoryModel
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public bool ISActive { get; set; }

    }
}