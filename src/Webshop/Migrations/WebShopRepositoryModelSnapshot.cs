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

            modelBuilder.Entity("Webshop.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductCategoryId");

                    b.Property<string>("ProductName");

                    b.Property<decimal>("ProductPrice");

                    b.HasKey("ProductID");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Webshop.Models.ProductCategory", b =>
                {
                    b.Property<int>("ProductCategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProductCategoryName");

                    b.HasKey("ProductCategoryID");

                    b.ToTable("ProductsCategories");
                });

            modelBuilder.Entity("Webshop.Models.Product", b =>
                {
                    b.HasOne("Webshop.Models.ProductCategory", "ProductCategory")
                        .WithMany("Product")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
