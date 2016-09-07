using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class WebShop3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CategoryID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Images_ImageID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Products_ProductID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_SubCategories_SubCategoryID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Vendors_VendorID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CategoryID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ImageID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ProductID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_SubCategoryID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_VendorID",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "CategoryForeignKey",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageForeignKey",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductForeignKey",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCatForeignKey",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorForeignKey",
                table: "Articles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryForeignKey",
                table: "Articles",
                column: "CategoryForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ImageForeignKey",
                table: "Articles",
                column: "ImageForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ProductForeignKey",
                table: "Articles",
                column: "ProductForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_SubCatForeignKey",
                table: "Articles",
                column: "SubCatForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_VendorForeignKey",
                table: "Articles",
                column: "VendorForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CategoryForeignKey",
                table: "Articles",
                column: "CategoryForeignKey",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Images_ImageForeignKey",
                table: "Articles",
                column: "ImageForeignKey",
                principalTable: "Images",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Products_ProductForeignKey",
                table: "Articles",
                column: "ProductForeignKey",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_SubCategories_SubCatForeignKey",
                table: "Articles",
                column: "SubCatForeignKey",
                principalTable: "SubCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Vendors_VendorForeignKey",
                table: "Articles",
                column: "VendorForeignKey",
                principalTable: "Vendors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CategoryForeignKey",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Images_ImageForeignKey",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Products_ProductForeignKey",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_SubCategories_SubCatForeignKey",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Vendors_VendorForeignKey",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_CategoryForeignKey",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ImageForeignKey",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ProductForeignKey",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_SubCatForeignKey",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_VendorForeignKey",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CategoryForeignKey",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ImageForeignKey",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ProductForeignKey",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "SubCatForeignKey",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "VendorForeignKey",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "ImageID",
                table: "Articles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryID",
                table: "Articles",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ImageID",
                table: "Articles",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ProductID",
                table: "Articles",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_SubCategoryID",
                table: "Articles",
                column: "SubCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_VendorID",
                table: "Articles",
                column: "VendorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CategoryID",
                table: "Articles",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Images_ImageID",
                table: "Articles",
                column: "ImageID",
                principalTable: "Images",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Products_ProductID",
                table: "Articles",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_SubCategories_SubCategoryID",
                table: "Articles",
                column: "SubCategoryID",
                principalTable: "SubCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Vendors_VendorID",
                table: "Articles",
                column: "VendorID",
                principalTable: "Vendors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
