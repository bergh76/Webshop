using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models
{
    public class ImageModel
    {
        [Key]
        public int ID { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public DateTime ImageDate { get; set; }
        public Guid ArticleGuid { get; set; }

    }
}
