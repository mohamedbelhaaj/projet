using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class FixedTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taches_Projets_ProjetIdProjet",
                table: "Taches");

            migrationBuilder.DropForeignKey(
                name: "FK_Taches_StatutTaches_StatutTacheIdStatutTache",
                table: "Taches");

            migrationBuilder.DropForeignKey(
                name: "FK_Taches_Users_UserIdUser",
                table: "Taches");

            migrationBuilder.DropIndex(
                name: "IX_Taches_ProjetIdProjet",
                table: "Taches");

            migrationBuilder.DropIndex(
                name: "IX_Taches_StatutTacheIdStatutTache",
                table: "Taches");

            migrationBuilder.DropIndex(
                name: "IX_Taches_UserIdUser",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "IdProjet",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "IdStatutTache",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "ProjetIdProjet",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "StatutTacheIdStatutTache",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "UserIdUser",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "Client",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Jeudi",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Lundi",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Mardi",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Mercredi",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Phase",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Projet",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Vendredi",
                table: "DetailImputations");

            migrationBuilder.AddColumn<string>(
                name: "ProjetId",
                table: "Taches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatutTacheId",
                table: "Taches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Taches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Charge",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Friday",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdClient",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdProjet",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdStatutTache",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdTache",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Monday",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thursday",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tuesday",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wednesday",
                table: "DetailImputations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taches_ProjetId",
                table: "Taches",
                column: "ProjetId");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_StatutTacheId",
                table: "Taches",
                column: "StatutTacheId");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_UserId",
                table: "Taches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "IdProjet",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_StatutTaches_StatutTacheId",
                table: "Taches",
                column: "StatutTacheId",
                principalTable: "StatutTaches",
                principalColumn: "IdStatutTache",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_Users_UserId",
                table: "Taches",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taches_Projets_ProjetId",
                table: "Taches");

            migrationBuilder.DropForeignKey(
                name: "FK_Taches_StatutTaches_StatutTacheId",
                table: "Taches");

            migrationBuilder.DropForeignKey(
                name: "FK_Taches_Users_UserId",
                table: "Taches");

            migrationBuilder.DropIndex(
                name: "IX_Taches_ProjetId",
                table: "Taches");

            migrationBuilder.DropIndex(
                name: "IX_Taches_StatutTacheId",
                table: "Taches");

            migrationBuilder.DropIndex(
                name: "IX_Taches_UserId",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "ProjetId",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "StatutTacheId",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Taches");

            migrationBuilder.DropColumn(
                name: "Charge",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Friday",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "IdClient",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "IdProjet",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "IdStatutTache",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "IdTache",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Monday",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Thursday",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Tuesday",
                table: "DetailImputations");

            migrationBuilder.DropColumn(
                name: "Wednesday",
                table: "DetailImputations");

            migrationBuilder.AddColumn<string>(
                name: "IdProjet",
                table: "Taches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdStatutTache",
                table: "Taches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdUser",
                table: "Taches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjetIdProjet",
                table: "Taches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatutTacheIdStatutTache",
                table: "Taches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdUser",
                table: "Taches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Jeudi",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lundi",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mardi",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mercredi",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phase",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Projet",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendredi",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taches_ProjetIdProjet",
                table: "Taches",
                column: "ProjetIdProjet");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_StatutTacheIdStatutTache",
                table: "Taches",
                column: "StatutTacheIdStatutTache");

            migrationBuilder.CreateIndex(
                name: "IX_Taches_UserIdUser",
                table: "Taches",
                column: "UserIdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_Projets_ProjetIdProjet",
                table: "Taches",
                column: "ProjetIdProjet",
                principalTable: "Projets",
                principalColumn: "IdProjet",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_StatutTaches_StatutTacheIdStatutTache",
                table: "Taches",
                column: "StatutTacheIdStatutTache",
                principalTable: "StatutTaches",
                principalColumn: "IdStatutTache",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Taches_Users_UserIdUser",
                table: "Taches",
                column: "UserIdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
