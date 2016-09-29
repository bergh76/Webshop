using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop.ViewModels
{
    public class ArticlesViewModel
    {
        public int ArticleId { get; set; }

        [Display(Name = "ArticleName", ResourceType = typeof(Resources.ArticlesViewModel))]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "ErrorArticleName")]
        public string ArticleName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.ArticlesViewModel))]
        [DataType(DataType.Text)]
        [StringLength(110)]
        [Required(ErrorMessage = "ErrorArticleDescription")]
        public string ArticleShortText { get; set; }

        [Display(Name = "Feature1", ResourceType = typeof(Resources.ArticlesViewModel))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesOne { get; set; }

        [Display(Name = "Feature2", ResourceType = typeof(Resources.ArticlesViewModel))]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesTwo { get; set; }

        [Display(Name = "Feature3", ResourceType = typeof(Resources.ArticlesViewModel))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesThree { get; set; }

        [Display(Name = "Feature4", ResourceType = typeof(Resources.ArticlesViewModel))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]        
        public string ArticleFeaturesFour { get; set; }

        [Display(Name = "Price", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Currency)]
        [Range(0, 99999)]
        [Required(ErrorMessage = "ErrorArticlePrice")]
        public decimal ArticlePrice { get; set; } //lang independent  

        public string ArticleNumber { get; set; } //lang independent  

        [Display(Name = "Stock", ResourceType = typeof(Resources.Articles))] 
        [Required(ErrorMessage = "ErrorArticleStock")]
        public int ArticleStock { get; set; } //lang independent  

        public VendorModel _Vendor { get; set; }
        [Display(Name = "Brand", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorVendorName")]

        public int VendorID { get; set; } //lang independent

        public ProductModel _Product { get; set; }
        [Display(Name = "Product", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorProductName")]
        public int ProductID { get; set; } //lang independent

        public CategoryModel _Category { get; set; }
        [Display(Name = "Category", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorCategoryName")]
        public int CategoryID { get; set; } //lang independent

        public SubCategoryModel _SubCategory { get; set; }
        [Display(Name = "SubCategoryType", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorSubCategory")]
        public int SubCategoryID { get; set; } //lang independent

        public ImageModel _Image { get; set; } //lang independent  
        //public int ImageID { get; set;}
        public bool ISCampaign { get; set; } //lang independent  
        public bool ISActive { get; set; } //lang independent  
        public bool ISTranslated { get; set; }
        public string ArticleImgPath { get; set; } //lang independent  
        public Guid ArticleGuid { get; set; } //lang independent  
        public int ImageId { get; set; }
        public string LangCode { get; set; }
    }
}
