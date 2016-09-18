using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models
{
    public class ArticleTranslation
    {
        [Key]
        public int ArticleId { get; set; }

        public string LangCode { get; set; }

        [Display(Name = "ArticelNumber")] //lang independent
        [DataType(DataType.Text)]
        public string ArticleNumber { get; set; }

        [Display(Name = "ArticleName", ResourceType = typeof(Resources.ArticlesTranslation))]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "ErrorArticleName")]
        public string ArticleName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resources.ArticlesTranslation))]
        [DataType(DataType.Text)]
        [StringLength(40)]
        [Required(ErrorMessage = "ErrorArticleDescription")]
        public string ArticleShortText { get; set; }

        [Display(Name = "Feature1", ResourceType = typeof(Resources.ArticlesTranslation))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesOne { get; set; }

        [Display(Name = "Feature2", ResourceType = typeof(Resources.ArticlesTranslation))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesTwo { get; set; }

        [Display(Name = "Feature3", ResourceType = typeof(Resources.ArticlesTranslation))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesThree { get; set; }

        [Display(Name = "Feature4", ResourceType = typeof(Resources.ArticlesTranslation))]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "ErrorProductdata")]
        public string ArticleFeaturesFour { get; set; }

        //public Guid ArticleGuid { get; set; }
        public bool ISTranslated { get; set; }
    }
}
