using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class Checkout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ArticleName",
                table: "OrderDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArticleTranslateArticleId",
                table: "OrderDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleTranslateLangCode",
                table: "OrderDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ArticleTranslateArticleId_ArticleTranslateLangCode",
                table: "OrderDetails",
                columns: new[] { "ArticleTranslateArticleId", "ArticleTranslateLangCode" });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ArticleTranslations_ArticleTranslateArticleId_ArticleTranslateLangCode",
                table: "OrderDetails",
                columns: new[] { "ArticleTranslateArticleId", "ArticleTranslateLangCode" },
                principalTable: "ArticleTranslations",
                principalColumns: new[] { "ArticleId", "LangCode" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ArticleTranslations_ArticleTranslateArticleId_ArticleTranslateLangCode",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ArticleTranslateArticleId_ArticleTranslateLangCode",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ArticleName",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ArticleTranslateArticleId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ArticleTranslateLangCode",
                table: "OrderDetails");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName");
        }
    }
}
