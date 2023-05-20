using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class projectprojetLivraison : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Projets_ProjetId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_DetailLivraisons_DetailLivraisonId",
                table: "Commentaires");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailLivraisons_Projets_ProjetId",
                table: "DetailLivraisons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailLivraisons",
                table: "DetailLivraisons");

     



            migrationBuilder.DropIndex(
                name: "IX_Clients_ProjetId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DetailLivraisonId",
                table: "Commentaires");

            migrationBuilder.DropColumn(
                name: "ProjetId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TypeClientId",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "ProjetId",
                table: "DetailLivraisons",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DetailLivraisons",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "DetailLivraisons",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DetailLivraisonClientId",
                table: "Commentaires",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailLivraisonProjetId",
                table: "Commentaires",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailLivraisons",
                table: "DetailLivraisons",
                columns: new[] { "ProjetId", "ClientId" });

            migrationBuilder.CreateTable(
                name: "ProjetEquipe",
                columns: table => new
                {
                    ProjetId = table.Column<string>(nullable: false),
                    EquipeId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetEquipe", x => new { x.ProjetId, x.EquipeId });
                    table.ForeignKey(
                        name: "FK_ProjetEquipe_Equips_EquipeId",
                        column: x => x.EquipeId,
                        principalTable: "Equips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetEquipe_Projets_ProjetId",
                        column: x => x.ProjetId,
                        principalTable: "Projets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailLivraisons_ClientId",
                table: "DetailLivraisons",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Commentaires_DetailLivraisonProjetId_DetailLivraisonClientId",
                table: "Commentaires",
                columns: new[] { "DetailLivraisonProjetId", "DetailLivraisonClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjetEquipe_EquipeId",
                table: "ProjetEquipe",
                column: "EquipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_DetailLivraisons_DetailLivraisonProjetId_DetailLivraisonClientId",
                table: "Commentaires",
                columns: new[] { "DetailLivraisonProjetId", "DetailLivraisonClientId" },
                principalTable: "DetailLivraisons",
                principalColumns: new[] { "ProjetId", "ClientId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailLivraisons_Clients_ClientId",
                table: "DetailLivraisons",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailLivraisons_Projets_ProjetId",
                table: "DetailLivraisons",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_DetailLivraisons_DetailLivraisonProjetId_DetailLivraisonClientId",
                table: "Commentaires");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailLivraisons_Clients_ClientId",
                table: "DetailLivraisons");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailLivraisons_Projets_ProjetId",
                table: "DetailLivraisons");

            migrationBuilder.DropTable(
                name: "ProjetEquipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailLivraisons",
                table: "DetailLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_DetailLivraisons_ClientId",
                table: "DetailLivraisons");

            migrationBuilder.DropIndex(
                name: "IX_Commentaires_DetailLivraisonProjetId_DetailLivraisonClientId",
                table: "Commentaires");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "DetailLivraisons");

            migrationBuilder.DropColumn(
                name: "DetailLivraisonClientId",
                table: "Commentaires");

            migrationBuilder.DropColumn(
                name: "DetailLivraisonProjetId",
                table: "Commentaires");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DetailLivraisons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjetId",
                table: "DetailLivraisons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "DetailLivraisonId",
                table: "Commentaires",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjetId",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeClientId",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailLivraisons",
                table: "DetailLivraisons",
                column: "Id");

  

        

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ProjetId",
                table: "Clients",
                column: "ProjetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Projets_ProjetId",
                table: "Clients",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_DetailLivraisons_DetailLivraisonId",
                table: "Commentaires",
                column: "DetailLivraisonId",
                principalTable: "DetailLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailLivraisons_Projets_ProjetId",
                table: "DetailLivraisons",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
