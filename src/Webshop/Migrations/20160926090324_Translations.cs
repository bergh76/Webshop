using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class Translations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LangCode",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LangCode",
                table: "SubCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LangCode",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LangCode",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LangCode",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "LangCode",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "LangCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LangCode",
                table: "Categories");
        }
    }
}
