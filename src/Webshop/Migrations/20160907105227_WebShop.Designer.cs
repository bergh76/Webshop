using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Webshop.Models;

namespace Webshop.Migrations
{
    [DbContext(typeof(WebShopRepository))]
    [Migration("20160907105227_WebShop")]
    partial class WebShop
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Webshop.Models.ArticleModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ArticleAddDate");

                    b.Property<string>("ArticleFeaturesFour");

                    b.Property<string>("ArticleFeaturesOne");

                    b.Property<string>("ArticleFeaturesThree");

                    b.Property<string>("ArticleFeaturesTwo");

                    b.Property<string>("ArticleGuid");

                    b.Property<string>("ArticleName");

                    b.Property<string>("ArticleNumber");

                    b.Property<decimal>("ArticlePrice");

                    b.Property<string>("ArticleShortText");

                    b.Property<int>("ArticleStock");

                    b.Property<int>("CategoryID");

                    b.Property<bool>("ISActive");

                    b.Property<bool>("ISCampaign");

                    b.Property<int?>("ImageID");

                    b.Property<int>("ProductID");

                    b.Property<int>("ProductImgPathID");

                    b.Property<int>("SubCategoryID");

                    b.Property<int>("VendorID");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("ImageID");

                    b.HasIndex("ProductID");

                    b.HasIndex("SubCategoryID");

                    b.HasIndex("VendorID");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Webshop.Models.CategoryModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryName");

                    b.Property<bool>("ISActive");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Webshop.Models.ImageModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ImageDate");

                    b.Property<string>("ImageName");

                    b.Property<string>("ImagePath");

                    b.HasKey("ID");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Webshop.Models.ProductModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ISActive");

                    b.Property<string>("ProductName");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Webshop.Models.SubCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ISActive");

                    b.Property<string>("SubCategoryName");

                    b.HasKey("ID");

                    b.ToTable("SubCategoryies");
                });

            modelBuilder.Entity("Webshop.Models.VendorModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ISActive");

                    b.Property<string>("VendorName");

                    b.Property<string>("VendorWebPage");

                    b.HasKey("ID");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Webshop.Models.ArticleModel", b =>
                {
                    b.HasOne("Webshop.Models.CategoryModel", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Webshop.Models.ImageModel", "Image")
                        .WithMany()
                        .HasForeignKey("ImageID");

                    b.HasOne("Webshop.Models.ProductModel", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Webshop.Models.SubCategory", "SubCategory")
                        .WithMany()
                        .HasForeignKey("SubCategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Webshop.Models.VendorModel", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
