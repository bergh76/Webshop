using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Webshop.Migrations
{
    public partial class WebShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(nullable: true),
                    ISActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageDate = table.Column<DateTime>(nullable: false),
                    ImageName = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ISActive = table.Column<bool>(nullable: false),
                    ProductName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SubCategoryies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ISActive = table.Column<bool>(nullable: false),
                    SubCategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategoryies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ISActive = table.Column<bool>(nullable: false),
                    VendorName = table.Column<string>(nullable: true),
                    VendorWebPage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleAddDate = table.Column<DateTime>(nullable: false),
                    ArticleFeaturesFour = table.Column<string>(nullable: true),
                    ArticleFeaturesOne = table.Column<string>(nullable: true),
                    ArticleFeaturesThree = table.Column<string>(nullable: true),
                    ArticleFeaturesTwo = table.Column<string>(nullable: true),
                    ArticleGuid = table.Column<string>(nullable: true),
                    ArticleName = table.Column<string>(nullable: true),
                    ArticleNumber = table.Column<string>(nullable: true),
                    ArticlePrice = table.Column<decimal>(nullable: false),
                    ArticleShortText = table.Column<string>(nullable: true),
                    ArticleStock = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    ISActive = table.Column<bool>(nullable: false),
                    ISCampaign = table.Column<bool>(nullable: false),
                    ImageID = table.Column<int>(nullable: true),
                    ProductID = table.Column<int>(nullable: false),
                    ProductImgPathID = table.Column<int>(nullable: false),
                    SubCategoryID = table.Column<int>(nullable: false),
                    VendorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Images_ImageID",
                        column: x => x.ImageID,
                        principalTable: "Images",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_SubCategoryies_SubCategoryID",
                        column: x => x.SubCategoryID,
                        principalTable: "SubCategoryies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SubCategoryies");

            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
