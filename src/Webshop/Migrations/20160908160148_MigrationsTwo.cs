using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class MigrationsTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductModelID",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductModelID",
                table: "Products",
                column: "ProductModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Products_ProductModelID",
                table: "Products",
                column: "ProductModelID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Products_ProductModelID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductModelID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductModelID",
                table: "Products");
        }
    }
}
