using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class WebshopTranslationThree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISDefault",
                table: "ArticleTranslations");

            migrationBuilder.AddColumn<bool>(
                name: "ISTranslated",
                table: "ArticleTranslations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISTranslated",
                table: "ArticleTranslations");

            migrationBuilder.AddColumn<bool>(
                name: "ISDefault",
                table: "ArticleTranslations",
                nullable: false,
                defaultValue: false);
        }
    }
}
