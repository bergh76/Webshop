using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop.ViewModels
{
    public class OrderOverviewViewModel
    {
        public IEnumerable<OrderDetail> OrderDetailList { get; set; }
        //public Order Order { get; set; }
        //public OrderDetail OrderDetails { get; set; }
        public int OrderId { get; set; }
        public int ArticleId { get; set; }
        public string ArticleNumber { get; set; }
        public string ArticleName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public string KlarnaOrderId { get; set; }
        public string UserId { get; set; }
        public virtual Articles Article { get; set; }
        public virtual ArticleTranslation ArticleTranslate { get; set; }

    }
}
