using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Webshop.Migrations
{
    public partial class SetKlarnaOrderIdToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleName",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "KlarnaOrderId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "KlarnaOrderId",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KlarnaOrderId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "ArticleName",
                table: "OrderDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KlarnaOrderId",
                table: "OrderDetails",
                nullable: true);
        }
    }
}
