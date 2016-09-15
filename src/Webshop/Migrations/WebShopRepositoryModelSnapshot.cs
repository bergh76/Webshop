using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Webshop.Models;

namespace Webshop.Migrations
{
    [DbContext(typeof(WebShopRepository))]
    partial class WebShopRepositoryModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Webshop.Models.Articles", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ArticleAddDate");

                    b.Property<Guid>("ArticleGuid");

                    b.Property<string>("ArticleNumber");

                    b.Property<decimal>("ArticlePrice");

                    b.Property<int>("ArticleStock");

                    b.Property<int?>("CategoryForeignKey");

                    b.Property<int>("CategoryId");

                    b.Property<bool>("ISActive");

                    b.Property<bool>("ISCampaign");

                    b.Property<int?>("ImageForeignKey");

                    b.Property<int>("ImageId");

                    b.Property<int?>("ProductForeignKey");

                    b.Property<string>("ProductId")
                        .IsRequired();

                    b.Property<int?>("SubCatForeignKey");

                    b.Property<int>("SubCategoryId");

                    b.Property<int?>("VendorForeignKey");

                    b.Property<int>("VendorId");

                    b.HasKey("ArticleId");

                    b.HasIndex("CategoryForeignKey");

                    b.HasIndex("ImageForeignKey");

                    b.HasIndex("ProductForeignKey");

                    b.HasIndex("SubCatForeignKey");

                    b.HasIndex("VendorForeignKey");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Webshop.Models.ArticleTranslation", b =>
                {
                    b.Property<int>("ArticleId");

                    b.Property<string>("LangCode");

                    b.Property<string>("ArticleFeaturesFour")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 66);

                    b.Property<string>("ArticleFeaturesOne")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 66);

                    b.Property<string>("ArticleFeaturesThree")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 66);

                    b.Property<string>("ArticleFeaturesTwo")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 66);

                    b.Property<string>("ArticleName")
                        .IsRequired();

                    b.Property<string>("ArticleNumber");

                    b.Property<string>("ArticleShortText")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 40);

                    b.Property<int?>("ArticlesArticleId");

                    b.Property<bool>("ISDefault");

                    b.HasKey("ArticleId", "LangCode");

                    b.HasIndex("ArticlesArticleId");

                    b.ToTable("ArticleTranslations");
                });

            modelBuilder.Entity("Webshop.Models.CategoryModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("CategoryName")
                        .IsRequired();

                    b.Property<bool>("ISActive");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Webshop.Models.ImageModel", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ArticleGuid");

                    b.Property<DateTime>("ImageDate");

                    b.Property<string>("ImageName");

                    b.Property<string>("ImagePath");

                    b.HasKey("ImageId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Webshop.Models.Language", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Country");

                    b.Property<string>("LangCode")
                        .HasAnnotation("MaxLength", 5);

                    b.HasKey("ID");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Webshop.Models.ProductModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ISActive");

                    b.Property<string>("ProductID");

                    b.Property<string>("ProductName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Webshop.Models.SubCategoryModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ISActive");

                    b.Property<int>("SubCategoryID");

                    b.Property<string>("SubCategoryName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("Webshop.Models.VendorModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ISActive");

                    b.Property<int>("VendorID");

                    b.Property<string>("VendorName")
                        .IsRequired();

                    b.Property<string>("VendorWebPage");

                    b.HasKey("ID");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Webshop.Models.Articles", b =>
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

                    b.HasOne("Webshop.Models.SubCategoryModel", "SubCategory")
                        .WithMany()
                        .HasForeignKey("SubCatForeignKey");

                    b.HasOne("Webshop.Models.VendorModel", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorForeignKey");
                });

            modelBuilder.Entity("Webshop.Models.ArticleTranslation", b =>
                {
                    b.HasOne("Webshop.Models.Articles")
                        .WithMany("Translations")
                        .HasForeignKey("ArticlesArticleId");
                });
        }
    }
}
