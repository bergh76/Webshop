using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Webshop.Models;

namespace Webshop.Migrations
{
    [DbContext(typeof(WebShopRepository))]
    [Migration("20161006141017_Asdf")]
    partial class Asdf
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Webshop.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Webshop.Models.Articles", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ArticleAddDate");

                    b.Property<Guid>("ArticleGuid");

                    b.Property<string>("ArticleNumber");

                    b.Property<decimal>("ArticlePrice");

                    b.Property<int>("ArticleStock");

                    b.Property<int>("CategoryId");

                    b.Property<bool>("ISActive");

                    b.Property<bool>("ISCampaign");

                    b.Property<int>("ImageId");

                    b.Property<int>("ProductId");

                    b.Property<int>("SubCategoryId");

                    b.Property<int>("VendorId");

                    b.Property<int?>("_CategoryID");

                    b.Property<int?>("_ProductID");

                    b.Property<int?>("_SubCategoryID");

                    b.Property<int?>("_VendorID");

                    b.HasKey("ArticleId");

                    b.HasIndex("ImageId");

                    b.HasIndex("_CategoryID");

                    b.HasIndex("_ProductID");

                    b.HasIndex("_SubCategoryID");

                    b.HasIndex("_VendorID");

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
                        .HasAnnotation("MaxLength", 110);

                    b.Property<int>("ArticleTranslationId");

                    b.Property<bool>("ISTranslated");

                    b.HasKey("ArticleId", "LangCode");

                    b.HasIndex("ArticleId");

                    b.ToTable("ArticleTranslations");
                });

            modelBuilder.Entity("Webshop.Models.CartItem", b =>
                {
                    b.Property<int>("CartItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArticleId");

                    b.Property<string>("ArticleName");

                    b.Property<string>("ArticleNumber");

                    b.Property<int?>("ArticleTranslationArticleId");

                    b.Property<string>("ArticleTranslationLangCode");

                    b.Property<string>("CartId")
                        .IsRequired();

                    b.Property<int>("Count");

                    b.Property<DateTime>("DateCreated");

                    b.Property<bool>("ISCheckedOut");

                    b.HasKey("CartItemId");

                    b.HasIndex("ArticleId");

                    b.HasIndex("ArticleTranslationArticleId", "ArticleTranslationLangCode");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("Webshop.Models.CategoryModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("CategoryName")
                        .IsRequired();

                    b.Property<bool>("ISActive");

                    b.Property<string>("LangCode");

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

            modelBuilder.Entity("Webshop.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("OrderDate");

                    b.Property<string>("Phone");

                    b.Property<string>("PostalCode");

                    b.Property<string>("State");

                    b.Property<decimal>("Total");

                    b.Property<string>("UserId");

                    b.Property<string>("Username");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Webshop.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArticleId");

                    b.Property<string>("ArticleName");

                    b.Property<string>("ArticleNumber");

                    b.Property<int?>("ArticleTranslateArticleId");

                    b.Property<string>("ArticleTranslateLangCode");

                    b.Property<string>("KlarnaOrderId");

                    b.Property<int>("OrderId");

                    b.Property<int>("Quantity");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("ArticleId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ArticleTranslateArticleId", "ArticleTranslateLangCode");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Webshop.Models.ProductModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ISActive");

                    b.Property<string>("LangCode");

                    b.Property<int>("ProductID");

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

                    b.Property<string>("LangCode");

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

                    b.Property<string>("LangCode");

                    b.Property<int>("VendorID");

                    b.Property<string>("VendorName")
                        .IsRequired();

                    b.Property<string>("VendorWebPage");

                    b.HasKey("ID");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Webshop.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Webshop.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Webshop.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Webshop.Models.Articles", b =>
                {
                    b.HasOne("Webshop.Models.ImageModel", "_Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Webshop.Models.CategoryModel", "_Category")
                        .WithMany()
                        .HasForeignKey("_CategoryID");

                    b.HasOne("Webshop.Models.ProductModel", "_Product")
                        .WithMany()
                        .HasForeignKey("_ProductID");

                    b.HasOne("Webshop.Models.SubCategoryModel", "_SubCategory")
                        .WithMany()
                        .HasForeignKey("_SubCategoryID");

                    b.HasOne("Webshop.Models.VendorModel", "_Vendor")
                        .WithMany()
                        .HasForeignKey("_VendorID");
                });

            modelBuilder.Entity("Webshop.Models.ArticleTranslation", b =>
                {
                    b.HasOne("Webshop.Models.Articles")
                        .WithMany("Translations")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Webshop.Models.CartItem", b =>
                {
                    b.HasOne("Webshop.Models.Articles", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Webshop.Models.ArticleTranslation", "ArticleTranslation")
                        .WithMany()
                        .HasForeignKey("ArticleTranslationArticleId", "ArticleTranslationLangCode");
                });

            modelBuilder.Entity("Webshop.Models.OrderDetail", b =>
                {
                    b.HasOne("Webshop.Models.Articles", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Webshop.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Webshop.Models.ArticleTranslation", "ArticleTranslate")
                        .WithMany()
                        .HasForeignKey("ArticleTranslateArticleId", "ArticleTranslateLangCode");
                });
        }
    }
}
