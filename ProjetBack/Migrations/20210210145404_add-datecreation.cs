using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class adddatecreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "createur",
                table: "CodeProjet",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateCreation",
                table: "CodeProjet",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createur",
                table: "CodeProjet");

            migrationBuilder.DropColumn(
                name: "dateCreation",
                table: "CodeProjet");
        }
    }
}
