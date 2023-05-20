using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class AbstractProjectAndDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EbRc",
                table: "ProjetLivraisons",
                newName: "EBRC");

            migrationBuilder.AddColumn<string>(
                name: "StatusId",
                table: "ProjetLivraisons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TTMId",
                table: "ProjetLivraisons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjetLivraisonId",
                table: "Commentaires",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetLivraisons_StatusId",
                table: "ProjetLivraisons",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetLivraisons_TTMId",
                table: "ProjetLivraisons",
                column: "TTMId");

            migrationBuilder.CreateIndex(
                name: "IX_Commentaires_ProjetLivraisonId",
                table: "Commentaires",
                column: "ProjetLivraisonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_ProjetLivraisons_ProjetLivraisonId",
                table: "Commentaires",
                column: "ProjetLivraisonId",
                principalTable: "ProjetLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetLivraisons_Status_StatusId",
                table: "ProjetLivraisons",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "IdStatus",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetLivraisons_TTM_TTMId",
                table: "ProjetLivraisons",
                column: "TTMId",
                principalTable: "TTM",
                principalColumn: "IdTTM",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_ProjetLivraisons_ProjetLivraisonId",
                table: "Commentaires");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetLivraisons_Status_StatusId",
                table: "ProjetLivraisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetLivraisons_TTM_TTMId",
                table: "ProjetLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_ProjetLivraisons_StatusId",
                table: "ProjetLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_ProjetLivraisons_TTMId",
                table: "ProjetLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_Commentaires_ProjetLivraisonId",
                table: "Commentaires");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ProjetLivraisons");

            migrationBuilder.DropColumn(
                name: "TTMId",
                table: "ProjetLivraisons");

            migrationBuilder.DropColumn(
                name: "ProjetLivraisonId",
                table: "Commentaires");

            migrationBuilder.RenameColumn(
                name: "EBRC",
                table: "ProjetLivraisons",
                newName: "EbRc");
        }
    }
}
