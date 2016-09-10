using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Models
{
    public class ArticleModel
    {
        [Key]
        public int ArticleID { get; set; }

        [Display(Name ="Artikelnummer")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Artikelnummer måste anges")]
        public string ArticleNumber { get; set; }

        [Display(Name = "Artikelnamn")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Artikelnamn måste anges")]
        public string ArticleName { get; set; }

        [Display(Name = "Styckpris")]
        [DataType(DataType.Currency)]
        [Range(0,99999)]
        [Required(ErrorMessage = "Pris måste anges")]
        public decimal ArticlePrice { get; set; }

        [Display(Name = "Lagersaldo")]
        [Required(ErrorMessage = "Lagerantal måste anges")]
        public int ArticleStock { get; set; }

        [Display(Name = "Beskrivning")]
        [DataType(DataType.Text)]
        [StringLength(40)]
        [Required(ErrorMessage = "Beskrivning måste anges")]
        public string ArticleShortText { get; set; }

        [Display(Name = "Produktdata 1")]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "Egenskap måste anges")]
        public string ArticleFeaturesOne { get; set; }

        [Display(Name = "Produktdata 2")]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "Egenskap måste anges")]
        public string ArticleFeaturesTwo { get; set; }

        [Display(Name = "Produktdata 3")]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "Egenskap måste anges")]
        public string ArticleFeaturesThree { get; set; }

        [Display(Name = "Produktdata 4")]
        [DataType(DataType.Text)]
        [StringLength(66)]
        [Required(ErrorMessage = "Egenskap måste anges")]
        public string ArticleFeaturesFour { get; set; }

        [Display(Name = "Aktiv [Ja/Nej]")]
        public bool ISActive { get; set; }

        [Display(Name = "Skapandedatum")]
        [DataType(DataType.Date)]
        public DateTime ArticleAddDate { get; set; }

        [Display(Name = "Kampanjvara [Ja/Nej]")]
        public bool ISCampaign { get; set; }
        public string ArticleGuid { get; set; }
        public string ArticleImgPath { get; set; }

        [ForeignKey("VendorForeignKey")]
        public VendorModel Vendor { get; set; }
        [Display(Name = "Tillverkare")]
        public int VendorID { get; set; }

        [ForeignKey("ProductForeignKey")]
        public ProductModel Product { get; set; }
        [Display(Name = "Produkt")]
        public int ProductID { get; set; }

        [ForeignKey("CategoryForeignKey")]
        public CategoryModel Category { get; set; }
        [Display(Name = "Kategori")]
        public int CategoryID { get; set; }

        [ForeignKey("SubCatForeignKey")]
        public SubCategory SubCategory { get; set; }
        [Display(Name = "Underkategori")]
        public int SubCategoryID { get; set; }

        [ForeignKey("ImageForeignKey")]
        public ImageModel Image { get; set; }
        public int ProductImgPathID { get; set; }
    }
}