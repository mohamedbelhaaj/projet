using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class lockoutEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "lockoutEnabled",
                table: "Projets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "lockoutEnabled",
                table: "ProjetLivraisons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "lockoutEnabled",
                table: "Clients",
                nullable: false,
                defaultValue: false);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lockoutEnabled",
                table: "Projets");

            migrationBuilder.DropColumn(
                name: "lockoutEnabled",
                table: "ProjetLivraisons");

            migrationBuilder.DropColumn(
                name: "lockoutEnabled",
                table: "Clients");
        }
    }
}
