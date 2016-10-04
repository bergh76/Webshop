using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ArticleId { get; set; }
        public string ArticleNumber { get; set; }
        public string ArticleName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Articles Article { get; set; }
        public virtual ArticleTranslation ArticleTranslate { get; set; }
        public virtual Order Order { get; set; }

    }
}
