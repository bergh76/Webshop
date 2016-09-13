using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Models
{
    public class Articles
    {
        [Key]
        public int ArticleID { get; set; }

        [Display(Name = "ArticelNumber")]
        [DataType(DataType.Text)]
        public string ArticleNumber { get; set; }

        [Display(Name = "ArticleName", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "ErrorArticleName")]
        public string ArticleName { get; set; }

        [Display(Name = "Price", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Currency)]
        [Range(0, 99999)]
        [Required(ErrorMessage = "ErrorArticlePrice")]
        public decimal ArticlePrice { get; set; }

        [Display(Name = "Stock", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorArticleStock")]
        public int ArticleStock { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Text)]
        [StringLength(40)]
        [Required(ErrorMessage = "ErrorArticleDescription")]
        public string ArticleShortText { get; set; }

        [Display(Name = "Feature1", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesOne { get; set; }

        [Display(Name = "Feature2", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesTwo { get; set; }

        [Display(Name = "Feature3", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesThree { get; set; }

        [Display(Name = "Feature4", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesFour { get; set; }

        [Display(Name = "Active", ResourceType = typeof(Resources.Articles))]
        public bool ISActive { get; set; }

        [Display(Name = "Date/Time")]
        [DataType(DataType.Date)]
        public DateTime ArticleAddDate { get; set; }

        [Display(Name = "Campaign", ResourceType = typeof(Resources.Articles))]
        public bool ISCampaign { get; set; }
        public string ArticleGuid { get; set; }
        public string ArticleImgPath { get; set; }

        [ForeignKey("VendorForeignKey")]
        public VendorModel Vendor { get; set; }
        [Display(Name = "Brand", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorVendorName")]
        public int VendorID { get; set; }

        [ForeignKey("ProductForeignKey")]
        public ProductModel Product { get; set; }
        [Display(Name = "Product", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorProductName")]
        public string ProductID { get; set; }

        [ForeignKey("CategoryForeignKey")]
        public CategoryModel Category { get; set; }
        [Display(Name = "Category", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorCategoryName")]
        public int CategoryID { get; set; }

        [ForeignKey("SubCatForeignKey")]
        public SubCategoryModel SubCategory { get; set; }
        [Display(Name = "SubCategoryType", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorSubCategory")]
        public int SubCategoryID { get; set; }

        [ForeignKey("ImageForeignKey")]
        public ImageModel Image { get; set; }
        public int ProductImgPathID { get; set; }
    }
}