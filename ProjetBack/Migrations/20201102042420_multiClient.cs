using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class multiClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdClient",
                table: "Projets");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebut",
                table: "Projets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFin",
                table: "Projets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ProjetId",
                table: "Clients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ProjetId",
                table: "Clients",
                column: "ProjetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Projets_ProjetId",
                table: "Clients",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Projets_ProjetId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ProjetId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DateDebut",
                table: "Projets");

            migrationBuilder.DropColumn(
                name: "DateFin",
                table: "Projets");

            migrationBuilder.DropColumn(
                name: "ProjetId",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "IdClient",
                table: "Projets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
