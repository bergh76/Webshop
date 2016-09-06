using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Models
{
    public class ArticleModel
    {
        [Key]
        public int ID { get; set; }
        public string ArticleNumber { get; set; }
        public string ArticleName { get; set; }
        [DataType(DataType.Currency)]
        public decimal ArticlePrice { get; set; }
        public int ArticleStock { get; set; }
        public string ArticleShortText { get; set; }
        public string ArticleFeaturesOne { get; set; }
        public string ArticleFeaturesTwo { get; set; }
        public string ArticleFeaturesThree { get; set; }
        public string ArticleFeaturesFour { get; set; }        
        public bool ISActive { get; set; }
        [DataType(DataType.Date)]
        public DateTime ArticleAddDate { get; set; }
        public bool ISCampaign { get; set; }
        public string ArticleGuid { get; set; }
        public string ArticleImgPath { get; set; }
        public int VendorID { get; set; }
        public string ProductID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        [ForeignKey("VendorForeignKey")]
        public VendorModel Vendor { get; set; }

        [ForeignKey("ProductForeignKey")]
        public ProductModel Product { get; set; }

        [ForeignKey("CategoryForeignKey")]
        public CategoryModel Category { get; set; }

        [ForeignKey("SubCatForeignKey")]
        public SubCategory SubCategory { get; set; }

        [ForeignKey("ImageForeignKey")]
        public ImageModel Image { get; set; }

    }
}
