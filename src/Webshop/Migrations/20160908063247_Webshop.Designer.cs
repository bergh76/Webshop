using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Webshop.Models;

namespace Webshop.Migrations
{
    [DbContext(typeof(WebShopRepository))]
    [Migration("20160908063247_Webshop")]
    partial class Webshop
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Webshop.Models.ArticleModel", b =>
                {
                    b.Property<int>("ArticleID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ArticleAddDate");

                    b.Property<string>("ArticleFeaturesFour")
                        .IsRequired();

                    b.Property<string>("ArticleFeaturesOne")
                        .IsRequired();

                    b.Property<string>("ArticleFeaturesThree")
                        .IsRequired();

                    b.Property<string>("ArticleFeaturesTwo")
                        .IsRequired();

                    b.Property<string>("ArticleGuid");

                    b.Property<string>("ArticleImgPath");

                    b.Property<string>("ArticleName")
                        .IsRequired();

                    b.Property<string>("ArticleNumber")
                        .IsRequired();

                    b.Property<decimal>("ArticlePrice");

                    b.Property<string>("ArticleShortText")
                        .IsRequired();

                    b.Property<int>("ArticleStock");

                    b.Property<int?>("CategoryForeignKey");

                    b.Property<int>("CategoryID");

                    b.Property<bool>("ISActive");

                    b.Property<bool>("ISCampaign");

                    b.Property<int?>("ImageForeignKey");

                    b.Property<int?>("ProductForeignKey");

                    b.Property<int>("ProductID");

                    b.Property<int>("ProductImgPathID");

                    b.Property<int?>("SubCatForeignKey");

                    b.Property<int>("SubCategoryID");

                    b.Property<int?>("VendorForeignKey");

                    b.Property<int>("VendorID");

                    b.HasKey("ArticleID");

                    b.HasIndex("CategoryForeignKey");

                    b.HasIndex("ImageForeignKey");

                    b.HasIndex("ProductForeignKey");

                    b.HasIndex("SubCatForeignKey");

                    b.HasIndex("VendorForeignKey");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Webshop.Models.CategoryModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("CategoryName");

                    b.Property<bool>("ISActive");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Webshop.Models.ImageModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ArticleGuid");

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

                    b.Property<string>("ProductID");

                    b.Property<string>("ProductName");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Webshop.Models.SubCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ISActive");

                    b.Property<int>("SubCategoryID");

                    b.Property<string>("SubCategoryName");

                    b.HasKey("ID");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("Webshop.Models.VendorModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ISActive");

                    b.Property<int>("VendorID");

                    b.Property<string>("VendorName");

                    b.Property<string>("VendorWebPage");

                    b.HasKey("ID");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Webshop.Models.ArticleModel", b =>
                {
                    b.HasOne("Webshop.Models.CategoryModel", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryForeignKey");

                    b.HasOne("Webshop.Models.ImageModel", "Image")
                        .WithMany()
                        .HasForeignKey("ImageForeignKey");

                    b.HasOne("Webshop.Models.ProductModel", "Product")
                        .WithMany()
                        .HasForeignKey("ProductForeignKey");

                    b.HasOne("Webshop.Models.SubCategory", "SubCategory")
                        .WithMany()
                        .HasForeignKey("SubCatForeignKey");

                    b.HasOne("Webshop.Models.VendorModel", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorForeignKey");
                });
        }
    }
}
