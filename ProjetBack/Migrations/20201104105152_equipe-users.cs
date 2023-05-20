using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class equipeusers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEquipe_Equips_EquipeId",
                table: "ProjetEquipe");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEquipe_Projets_ProjetId",
                table: "ProjetEquipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjetEquipe",
                table: "ProjetEquipe");

            migrationBuilder.RenameTable(
                name: "ProjetEquipe",
                newName: "ProjetEquipes");

            migrationBuilder.RenameIndex(
                name: "IX_ProjetEquipe_EquipeId",
                table: "ProjetEquipes",
                newName: "IX_ProjetEquipes_EquipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjetEquipes",
                table: "ProjetEquipes",
                columns: new[] { "ProjetId", "EquipeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEquipes_Equips_EquipeId",
                table: "ProjetEquipes",
                column: "EquipeId",
                principalTable: "Equips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEquipes_Projets_ProjetId",
                table: "ProjetEquipes",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEquipes_Equips_EquipeId",
                table: "ProjetEquipes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEquipes_Projets_ProjetId",
                table: "ProjetEquipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjetEquipes",
                table: "ProjetEquipes");

            migrationBuilder.RenameTable(
                name: "ProjetEquipes",
                newName: "ProjetEquipe");

            migrationBuilder.RenameIndex(
                name: "IX_ProjetEquipes_EquipeId",
                table: "ProjetEquipe",
                newName: "IX_ProjetEquipe_EquipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjetEquipe",
                table: "ProjetEquipe",
                columns: new[] { "ProjetId", "EquipeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEquipe_Equips_EquipeId",
                table: "ProjetEquipe",
                column: "EquipeId",
                principalTable: "Equips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEquipe_Projets_ProjetId",
                table: "ProjetEquipe",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
