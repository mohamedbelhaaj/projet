using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class FixingRelationProjetLClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "ProjetLivraisons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "DetailImputations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetLivraisons_ClientId",
                table: "ProjetLivraisons",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetLivraisons_Clients_ClientId",
                table: "ProjetLivraisons",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetLivraisons_Clients_ClientId",
                table: "ProjetLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_ProjetLivraisons_ClientId",
                table: "ProjetLivraisons");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ProjetLivraisons");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DetailImputations");
        }
    }
}
