using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class Asdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ArticleTranslationId",
                table: "ArticleTranslations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTranslations_ArticleId",
                table: "ArticleTranslations",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTranslations_Articles_ArticleId",
                table: "ArticleTranslations",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTranslations_Articles_ArticleId",
                table: "ArticleTranslations");

            migrationBuilder.DropIndex(
                name: "IX_ArticleTranslations_ArticleId",
                table: "ArticleTranslations");

            migrationBuilder.DropColumn(
                name: "ArticleTranslationId",
                table: "ArticleTranslations");

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
    }
}
