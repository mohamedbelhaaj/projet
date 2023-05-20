using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class ProjetEdpsclients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "clientId",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetEdps_clientId",
                table: "ProjetEdps",
                column: "clientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEdps_Clients_clientId",
                table: "ProjetEdps",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEdps_Clients_clientId",
                table: "ProjetEdps");

            migrationBuilder.DropIndex(
                name: "IX_ProjetEdps_clientId",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "clientId",
                table: "ProjetEdps");
        }
    }
}
