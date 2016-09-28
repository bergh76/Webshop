using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class ProductID_string_2_int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "Products",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Articles",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductID",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Articles",
                nullable: false);
        }
    }
}
