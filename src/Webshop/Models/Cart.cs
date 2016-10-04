using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public string CartId { get; set; }
        public int ArticleId { get; set; }   
        public string ArticleNumber { get; set; }
        public string ArticleName { get; set; }  
        //public string ArticleImgPath { get; set; } 

        public int Count { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
         public bool ISCheckedOut { get; set; }
        public virtual Articles Article { get; set; }
        public virtual ArticleTranslation ArticleTranslation { get; set; }
    }
}