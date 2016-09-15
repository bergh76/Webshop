using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Webshop.Migrations
{
    public partial class WebshopTranslation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryID = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(nullable: false),
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
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleGuid = table.Column<Guid>(nullable: false),
                    ImageDate = table.Column<DateTime>(nullable: false),
                    ImageName = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Country = table.Column<string>(nullable: true),
                    LangCode = table.Column<string>(maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ISActive = table.Column<bool>(nullable: false),
                    ProductID = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ISActive = table.Column<bool>(nullable: false),
                    SubCategoryID = table.Column<int>(nullable: false),
                    SubCategoryName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ISActive = table.Column<bool>(nullable: false),
                    VendorID = table.Column<int>(nullable: false),
                    VendorName = table.Column<string>(nullable: false),
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
                    ArticleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleAddDate = table.Column<DateTime>(nullable: false),
                    ArticleGuid = table.Column<Guid>(nullable: false),
                    ArticleNumber = table.Column<string>(nullable: true),
                    ArticlePrice = table.Column<decimal>(nullable: false),
                    ArticleStock = table.Column<int>(nullable: false),
                    CategoryForeignKey = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    ISActive = table.Column<bool>(nullable: false),
                    ISCampaign = table.Column<bool>(nullable: false),
                    ImageForeignKey = table.Column<int>(nullable: true),
                    ImageId = table.Column<int>(nullable: false),
                    ProductForeignKey = table.Column<int>(nullable: true),
                    ProductId = table.Column<string>(nullable: false),
                    SubCatForeignKey = table.Column<int>(nullable: true),
                    SubCategoryId = table.Column<int>(nullable: false),
                    VendorForeignKey = table.Column<int>(nullable: true),
                    VendorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryForeignKey",
                        column: x => x.CategoryForeignKey,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Images_ImageForeignKey",
                        column: x => x.ImageForeignKey,
                        principalTable: "Images",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Products_ProductForeignKey",
                        column: x => x.ProductForeignKey,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_SubCategories_SubCatForeignKey",
                        column: x => x.SubCatForeignKey,
                        principalTable: "SubCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Vendors_VendorForeignKey",
                        column: x => x.VendorForeignKey,
                        principalTable: "Vendors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTranslations",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    LangCode = table.Column<string>(nullable: false),
                    ArticleFeaturesFour = table.Column<string>(maxLength: 66, nullable: false),
                    ArticleFeaturesOne = table.Column<string>(maxLength: 66, nullable: false),
                    ArticleFeaturesThree = table.Column<string>(maxLength: 66, nullable: false),
                    ArticleFeaturesTwo = table.Column<string>(maxLength: 66, nullable: false),
                    ArticleName = table.Column<string>(nullable: false),
                    ArticleNumber = table.Column<string>(nullable: true),
                    ArticleShortText = table.Column<string>(maxLength: 40, nullable: false),
                    ArticlesArticleId = table.Column<int>(nullable: true),
                    ISDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTranslations", x => new { x.ArticleId, x.LangCode });
                    table.ForeignKey(
                        name: "FK_ArticleTranslations_Articles_ArticlesArticleId",
                        column: x => x.ArticlesArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTranslations_ArticlesArticleId",
                table: "ArticleTranslations",
                column: "ArticlesArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTranslations");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
