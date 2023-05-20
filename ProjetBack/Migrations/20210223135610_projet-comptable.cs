using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class projetcomptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "budgetConfirmeRallonge",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetConfirmé",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetDirection",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetDirectionRallonge",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetGP",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetGPRallonge",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetJunior",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetJuniorRallonge",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetSenior",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetSeniorRallonge",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetValidation",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "budgetValidationRallonge",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateDebut",
                table: "ProjetEdps",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "dateFin",
                table: "ProjetEdps",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "dateFinInitial",
                table: "ProjetEdps",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "budgetConfirmeRallonge",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetConfirmé",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetDirection",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetDirectionRallonge",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetGP",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetGPRallonge",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetJunior",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetJuniorRallonge",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetSenior",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetSeniorRallonge",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetValidation",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "budgetValidationRallonge",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "dateDebut",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "dateFin",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "dateFinInitial",
                table: "ProjetEdps");
        }
    }
}
