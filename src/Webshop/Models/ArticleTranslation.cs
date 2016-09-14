using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models
{
    public class ArticleTranslation
    {
        public int ArticleID { get;set;}
        public string Language { get; set; }
        public string ArticleName { get; set; }
        public string ArticleDescription { get; set; }
    }
}
