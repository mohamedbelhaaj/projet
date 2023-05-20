using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class projetEdp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetLivraisons_Projets_ProjetId",
                table: "ProjetLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_ProjetLivraisons_ProjetId",
                table: "ProjetLivraisons");

            migrationBuilder.DropColumn(
                name: "ProjetId",
                table: "ProjetLivraisons");

            migrationBuilder.AddColumn<string>(
                name: "ProjetName",
                table: "ProjetLivraisons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "projetEdpId",
                table: "ProjetLivraisons",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjetEdp",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    Nom = table.Column<string>(nullable: true),
                    budget = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetEdp", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjetLivraisons_projetEdpId",
                table: "ProjetLivraisons",
                column: "projetEdpId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetLivraisons_ProjetEdp_projetEdpId",
                table: "ProjetLivraisons",
                column: "projetEdpId",
                principalTable: "ProjetEdp",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetLivraisons_ProjetEdp_projetEdpId",
                table: "ProjetLivraisons");

            migrationBuilder.DropTable(
                name: "ProjetEdp");

            migrationBuilder.DropIndex(
                name: "IX_ProjetLivraisons_projetEdpId",
                table: "ProjetLivraisons");

            migrationBuilder.DropColumn(
                name: "ProjetName",
                table: "ProjetLivraisons");

            migrationBuilder.DropColumn(
                name: "projetEdpId",
                table: "ProjetLivraisons");

            migrationBuilder.AddColumn<string>(
                name: "ProjetId",
                table: "ProjetLivraisons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetLivraisons_ProjetId",
                table: "ProjetLivraisons",
                column: "ProjetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetLivraisons_Projets_ProjetId",
                table: "ProjetLivraisons",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
