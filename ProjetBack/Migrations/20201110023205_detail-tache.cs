using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class detailtache : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches");


            migrationBuilder.DropColumn(
                name: "ProjetId",
                table: "Taches");

            migrationBuilder.AddColumn<string>(
                name: "detailLivraisonId",
                table: "Taches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taches_detailLivraisonId",
                table: "Taches",
                column: "detailLivraisonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_DetailLivraisons_detailLivraisonId",
                table: "Taches",
                column: "detailLivraisonId",
                principalTable: "DetailLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taches_DetailLivraisons_detailLivraisonId",
                table: "Taches");

            migrationBuilder.DropIndex(
                name: "IX_Taches_detailLivraisonId",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "detailLivraisonId",
                table: "Taches");

            migrationBuilder.AddColumn<string>(
                name: "ProjetId",
                table: "Taches",
                type: "nvarchar(450)",
                nullable: true);


            migrationBuilder.AddForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
