using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class ArticleTranslateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleGuid",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ArticleNumber",
                table: "ArticleTranslations");

            migrationBuilder.AddColumn<int>(
                name: "ArtikelId",
                table: "Images",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtikelId",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "ArticleGuid",
                table: "Images",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ArticleNumber",
                table: "ArticleTranslations",
                nullable: true);
        }
    }
}
