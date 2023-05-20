using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class FixedProjet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projets_DetailLivraisons_Id",
                table: "Projets");

            migrationBuilder.DropForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projets",
                table: "Projets");

            migrationBuilder.DropIndex(
                name: "IX_Projets_Id",
                table: "Projets");

            migrationBuilder.DropColumn(
                name: "IdProjet",
                table: "Projets");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Projets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailLivraisoId",
                table: "Projets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectId",
                table: "DetailLivraisons",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projets",
                table: "Projets",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projets_DetailLivraisons_DetailLivraisoId",
                table: "Projets");

            migrationBuilder.DropForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projets",
                table: "Projets");

            migrationBuilder.DropIndex(
                name: "IX_Projets_DetailLivraisoId",
                table: "Projets");

            migrationBuilder.DropColumn(
                name: "DetailLivraisoId",
                table: "Projets");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "DetailLivraisons");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Projets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "IdProjet",
                table: "Projets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projets",
                table: "Projets",
                column: "IdProjet");

            migrationBuilder.CreateIndex(
                name: "IX_Projets_Id",
                table: "Projets",
                column: "Id",
                unique: true,
                filter: "[Id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Projets_DetailLivraisons_Id",
                table: "Projets",
                column: "Id",
                principalTable: "DetailLivraisons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "IdProjet",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
