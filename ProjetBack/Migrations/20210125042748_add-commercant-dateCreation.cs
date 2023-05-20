using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class addcommercantdateCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Createur",
                table: "Commercants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateCreation",
                table: "Commercants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Createur",
                table: "Commercants");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "Commercants");
        }
    }
}
