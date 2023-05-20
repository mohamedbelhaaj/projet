using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class FixingRelationDetailLivraisonProjet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projets_DetailLivraisons_DetailLivraisoId",
                table: "Projets");

            migrationBuilder.DropIndex(
                name: "IX_Projets_DetailLivraisoId",
                table: "Projets");

            migrationBuilder.DropColumn(
                name: "DetailLivraisoId",
                table: "Projets");

            migrationBuilder.AddColumn<string>(
                name: "ProjetId",
                table: "DetailLivraisons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetailLivraisons_ProjetId",
                table: "DetailLivraisons",
                column: "ProjetId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailLivraisons_Projets_ProjetId",
                table: "DetailLivraisons",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailLivraisons_Projets_ProjetId",
                table: "DetailLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_DetailLivraisons_ProjetId",
                table: "DetailLivraisons");

            migrationBuilder.DropColumn(
                name: "ProjetId",
                table: "DetailLivraisons");

            migrationBuilder.AddColumn<string>(
                name: "DetailLivraisoId",
                table: "Projets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projets_DetailLivraisoId",
                table: "Projets",
                column: "DetailLivraisoId",
                unique: true,
                filter: "[DetailLivraisoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Projets_DetailLivraisons_DetailLivraisoId",
                table: "Projets",
                column: "DetailLivraisoId",
                principalTable: "DetailLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
