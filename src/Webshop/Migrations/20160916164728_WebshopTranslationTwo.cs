using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Webshop.Migrations
{
    public partial class WebshopTranslationTwo : Migration
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
                    ImageName = table.Column<string>(nullable: false),
                    ArticleGuid = table.Column<Guid>(nullable: false),
                    ImagePath = table.Column<string>(nullable: false),
                    ImageDate = table.Column<DateTime>(nullable: false)
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
                    ProductID = table.Column<string>(nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    ISActive = table.Column<bool>(nullable: false)
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
                    SubCategoryID = table.Column<int>(nullable: false),
                    SubCategoryName = table.Column<string>(nullable: false),
                    ISActive = table.Column<bool>(nullable: false)
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
                    VendorID = table.Column<int>(nullable: false),
                    VendorName = table.Column<string>(nullable: false),
                    VendorWebPage = table.Column<string>(nullable: false),
                    ISActive = table.Column<bool>(nullable: false)
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
                    ArticleNumber = table.Column<string>(nullable: false),
                    ArticlePrice = table.Column<decimal>(nullable: false),
                    ArticleStock = table.Column<int>(nullable: false),
                    ArticleAddDate = table.Column<DateTime>(nullable: false),
                    ArticleGuid = table.Column<Guid>(nullable: false),
                    ISActive = table.Column<bool>(nullable: false),
                    ISCampaign = table.Column<bool>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),                   
                    ProductId = table.Column<string>(nullable: false),
                    SubCategoryId = table.Column<int>(nullable: false),
                    ImageId = table.Column<int>(nullable: false),
                    _VendorID = table.Column<int>(nullable: true),
                    _CategoryID = table.Column<int>(nullable: true),
                    _ProductID = table.Column<int>(nullable: true),
                    _SubCategoryID = table.Column<int>(nullable: true)

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Articles_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Categories__CategoryID",
                        column: x => x._CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Products__ProductID",
                        column: x => x._ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_SubCategories__SubCategoryID",
                        column: x => x._SubCategoryID,
                        principalTable: "SubCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Vendors__VendorID",
                        column: x => x._VendorID,
                        principalTable: "Vendors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTranslations",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    ArticleNumber = table.Column<string>(nullable: false),
                    LangCode = table.Column<string>(nullable: false),
                    ArticleName = table.Column<string>(nullable: false),
                    ArticlesArticleId = table.Column<int>(nullable: true),
                    ArticleShortText = table.Column<string>(maxLength: 40, nullable: false),
                    ArticleFeaturesOne = table.Column<string>(maxLength: 66, nullable: false),
                    ArticleFeaturesTwo = table.Column<string>(maxLength: 66, nullable: false),
                    ArticleFeaturesThree = table.Column<string>(maxLength: 66, nullable: false),
                    ArticleFeaturesFour = table.Column<string>(maxLength: 66, nullable: false),
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
                name: "IX_Articles_ImageId",
                table: "Articles",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles__CategoryID",
                table: "Articles",
                column: "_CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles__ProductID",
                table: "Articles",
                column: "_ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles__SubCategoryID",
                table: "Articles",
                column: "_SubCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles__VendorID",
                table: "Articles",
                column: "_VendorID");

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
                name: "Images");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
