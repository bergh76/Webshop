using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Webshop.Models
{
    public class ArticleModel
    {
        public int ID { get; set; }

        [Display(Name ="Artikelnummer")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Artikelnummer måste anges")]
        public string ArticleNumber { get; set; }

        [Display(Name = "Artikelnamn")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Artikelnamn måste anges")]
        public string ArticleName { get; set; }

        [Display(Name = "Pris")]
        [DataType(DataType.Currency)]
        [Range(0,99999)]
        [Required(ErrorMessage = "Pris måste anges")]
        public decimal ArticlePrice { get; set; }


        [Display(Name = "Lagestatus")]
        [Required(ErrorMessage = "Lagerantal måste anges")]
        public int ArticleStock { get; set; }

        [Display(Name = "Beskrivning")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Beskrivning måste anges")]
        public string ArticleShortText { get; set; }

        [Display(Name = "Egenskap1")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Egenskap måste anges")]
        public string ArticleFeaturesOne { get; set; }

        [Display(Name = "Egenskap2")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Egenskap måste anges")]
        public string ArticleFeaturesTwo { get; set; }

        [Display(Name = "Egenskap3")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Egenskap måste anges")]
        public string ArticleFeaturesThree { get; set; }

        [Display(Name = "Egenskap4")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Egenskap måste anges")]
        public string ArticleFeaturesFour { get; set; }

        [Display(Name = "Lagestatus")]
        public bool ISActive { get; set; }

        [Display(Name = "Skapandedatum")]
        [DataType(DataType.Date)]
        public DateTime ArticleAddDate { get; set; }

        public bool ISCampaign { get; set; }
        public string ArticleGuid { get; set; }

        //[ForeignKey("VendorForeignKey")]
        public VendorModel Vendor { get; set; }
        [Display(Name = "Tillverkare")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Tillverkare måste anges")]
        public int VendorID { get; set; }


        public ProductModel Product { get; set; }
        [Display(Name = "Produkttyp")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Produkttyp måste anges")]
        public int ProductID { get; set; }

        //[ForeignKey("CategoryForeignKey")]
        public CategoryModel Category { get; set; }
        [Display(Name = "Kategorityp")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Kategorityp måste anges")]
        public int CategoryID { get; set; }

        //[ForeignKey("SubCatForeignKey")]
        public SubCategory SubCategory { get; set; }
        [Display(Name = "Underkategori")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Underkategori måste anges")]
        public int SubCategoryID { get; set; }

        //[ForeignKey("ImageForeignKey")]
        public ImageModel Image { get; set; }
        public int ProductImgPathID { get; set; }
    }
}