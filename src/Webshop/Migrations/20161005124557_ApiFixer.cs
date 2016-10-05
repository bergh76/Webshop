using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class ApiFixer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticlesArticleId",
                table: "ArticleTranslations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTranslations_ArticlesArticleId",
                table: "ArticleTranslations",
                column: "ArticlesArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTranslations_Articles_ArticlesArticleId",
                table: "ArticleTranslations",
                column: "ArticlesArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTranslations_Articles_ArticlesArticleId",
                table: "ArticleTranslations");

            migrationBuilder.DropIndex(
                name: "IX_ArticleTranslations_ArticlesArticleId",
                table: "ArticleTranslations");

            migrationBuilder.DropColumn(
                name: "ArticlesArticleId",
                table: "ArticleTranslations");
        }
    }
}
