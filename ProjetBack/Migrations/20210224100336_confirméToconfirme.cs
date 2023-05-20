using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class confirméToconfirme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "budgetConfirmé",
                table: "ProjetEdps");

            migrationBuilder.AddColumn<string>(
                name: "budgetConfirme",
                table: "ProjetEdps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "budgetConfirme",
                table: "ProjetEdps");

            migrationBuilder.AddColumn<string>(
                name: "budgetConfirmé",
                table: "ProjetEdps",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
