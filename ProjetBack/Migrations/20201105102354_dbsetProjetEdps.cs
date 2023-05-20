using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class dbsetProjetEdps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetLivraisons_ProjetEdp_projetEdpId",
                table: "ProjetLivraisons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjetEdp",
                table: "ProjetEdp");

            migrationBuilder.RenameTable(
                name: "ProjetEdp",
                newName: "ProjetEdps");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjetEdps",
                table: "ProjetEdps",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetLivraisons_ProjetEdps_projetEdpId",
                table: "ProjetLivraisons",
                column: "projetEdpId",
                principalTable: "ProjetEdps",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetLivraisons_ProjetEdps_projetEdpId",
                table: "ProjetLivraisons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjetEdps",
                table: "ProjetEdps");

            migrationBuilder.RenameTable(
                name: "ProjetEdps",
                newName: "ProjetEdp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjetEdp",
                table: "ProjetEdp",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetLivraisons_ProjetEdp_projetEdpId",
                table: "ProjetLivraisons",
                column: "projetEdpId",
                principalTable: "ProjetEdp",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
