using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class detaillivraisonkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_DetailLivraisons_DetailLivraisonProjetId_DetailLivraisonClientId",
                table: "Commentaires");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailLivraisons",
                table: "DetailLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_Commentaires_DetailLivraisonProjetId_DetailLivraisonClientId",
                table: "Commentaires");

            migrationBuilder.DropColumn(
                name: "DetailLivraisonClientId",
                table: "Commentaires");

            migrationBuilder.DropColumn(
                name: "DetailLivraisonProjetId",
                table: "Commentaires");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DetailLivraisons",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailLivraisonId",
                table: "Commentaires",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailLivraisons",
                table: "DetailLivraisons",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DetailLivraisons_ProjetId",
                table: "DetailLivraisons",
                column: "ProjetId");

            migrationBuilder.CreateIndex(
                name: "IX_Commentaires_DetailLivraisonId",
                table: "Commentaires",
                column: "DetailLivraisonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_DetailLivraisons_DetailLivraisonId",
                table: "Commentaires",
                column: "DetailLivraisonId",
                principalTable: "DetailLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_DetailLivraisons_DetailLivraisonId",
                table: "Commentaires");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailLivraisons",
                table: "DetailLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_DetailLivraisons_ProjetId",
                table: "DetailLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_Commentaires_DetailLivraisonId",
                table: "Commentaires");

            migrationBuilder.DropColumn(
                name: "DetailLivraisonId",
                table: "Commentaires");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DetailLivraisons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "DetailLivraisonClientId",
                table: "Commentaires",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailLivraisonProjetId",
                table: "Commentaires",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailLivraisons",
                table: "DetailLivraisons",
                columns: new[] { "ProjetId", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_Commentaires_DetailLivraisonProjetId_DetailLivraisonClientId",
                table: "Commentaires",
                columns: new[] { "DetailLivraisonProjetId", "DetailLivraisonClientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_DetailLivraisons_DetailLivraisonProjetId_DetailLivraisonClientId",
                table: "Commentaires",
                columns: new[] { "DetailLivraisonProjetId", "DetailLivraisonClientId" },
                principalTable: "DetailLivraisons",
                principalColumns: new[] { "ProjetId", "ClientId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
