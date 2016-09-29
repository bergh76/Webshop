using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Models
{
    public class Articles
    {
        [Key]
        public int ArticleId { get; set; }

        [Display(Name = "ArticelNumber")] //lang independent
        [DataType(DataType.Text)]
        public string ArticleNumber { get; set; }

        [Display(Name = "Price", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Currency)]
        [Range(0, 99999)]
        [Required(ErrorMessage = "ErrorArticlePrice")]
        public decimal ArticlePrice { get; set; }

        [Display(Name = "Stock", ResourceType = typeof(Resources.Articles))] //lang independent
        [Required(ErrorMessage = "ErrorArticleStock")]
        public int ArticleStock { get; set; }

        [Display(Name = "Active", ResourceType = typeof(Resources.Articles))] //lang independent
        public bool ISActive { get; set; }

        [Display(Name = "Date/Time")] //lang independent
        [DataType(DataType.Date)]
        public DateTime ArticleAddDate { get; set; }

        [Display(Name = "Campaign", ResourceType = typeof(Resources.Articles))] //lang independent
        public bool ISCampaign { get; set; }

        public Guid ArticleGuid { get; set; } //lang independent

        public VendorModel _Vendor { get; set; }
        [Display(Name = "Brand", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorVendorName")]
        public int VendorId { get; set; } //lang independent

        public ProductModel _Product { get; set; }
        [Display(Name = "Product", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorProductName")]
        public int ProductId { get; set; } //lang independent

        public CategoryModel _Category { get; set; }
        [Display(Name = "Category", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorCategoryName")]
        public int CategoryId { get; set; } //lang independent

        public SubCategoryModel _SubCategory { get; set; }
        [Display(Name = "SubCategoryType", ResourceType = typeof(Resources.Articles))]
        [Required(ErrorMessage = "ErrorSubCategory")]
        public int SubCategoryId { get; set; } //lang independent

        public ImageModel _Image { get; set; }
        public int ImageId { get; set; } //lang independent

        [Required]
        public virtual ICollection<ArticleTranslation> Translations { get; set; }

    }
}