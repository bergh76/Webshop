using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop.ViewModels
{
    public class ArticlesViewModel
    {

        public int ArticleID { get; set; }

        public string ArticleName { get; set; }
        public string ArticleDescription { get; set; }
        public string ArticleFeaturesOne { get; set; }
        public string ArticleFeaturesTwo { get; set; }
        public string ArticleFeaturesThree { get; set; }
        public string ArticleFeaturesFour { get; set; }
        public decimal ArticlePrice { get; set; }
        public int ArticleStock { get; set; }
        public Guid ArticleGuid { get; set; }
        public VendorModel Vendor { get; set; }
        public int VendorID { get; set; } //lang independent

        public ProductModel Product { get; set; }
        public string ProductID { get; set; } //lang independent

        public CategoryModel Category { get; set; }
        public int CategoryID { get; set; } //lang independent

        public SubCategoryModel SubCategory { get; set; }
        public int SubCategoryID { get; set; } //lang independent

        public ImageModel Image { get; set; }
        public bool ISCampaign { get; set; }
        public bool ISActive { get; set; }
        public string ArticleImgPath { get; set; } //lang independent
        public DateTime ArticleAddDate { get; set; }

    }
}
