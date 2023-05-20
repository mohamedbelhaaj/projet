using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class adddatefinprevuettmtoprojetedp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dateFinPrevue",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ttm",
                table: "ProjetEdps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateFinPrevue",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "ttm",
                table: "ProjetEdps");
        }
    }
}
