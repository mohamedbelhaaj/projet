using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class addscopemigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "scope",
                table: "ProjetLivraisons",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateFinInitial",
                table: "ProjetEdps",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "scope",
                table: "ProjetLivraisons");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateFinInitial",
                table: "ProjetEdps",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
