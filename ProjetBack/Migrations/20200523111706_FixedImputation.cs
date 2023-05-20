using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class FixedImputation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationsIdStatusImputations",
                table: "Imputations");

            migrationBuilder.DropForeignKey(
                name: "FK_Imputations_Users_UserIdUser",
                table: "Imputations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Profils_ProfilIdProfil",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfilIdProfil",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Imputations_StatusImputationsIdStatusImputations",
                table: "Imputations");

            migrationBuilder.DropIndex(
                name: "IX_Imputations_UserIdUser",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "IdProfil",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilIdProfil",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdStatusImputations",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "StatusImputationsIdStatusImputations",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "UserIdUser",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "IdImputation",
                table: "DetailImputations");

            migrationBuilder.AddColumn<string>(
                name: "ProfilId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusImputationsId",
                table: "Imputations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Imputations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfilId",
                table: "Users",
                column: "ProfilId");

            migrationBuilder.CreateIndex(
                name: "IX_Imputations_StatusImputationsId",
                table: "Imputations",
                column: "StatusImputationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Imputations_UserId",
                table: "Imputations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationsId",
                table: "Imputations",
                column: "StatusImputationsId",
                principalTable: "StatusImputations",
                principalColumn: "IdStatusImputations",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imputations_Users_UserId",
                table: "Imputations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Profils_ProfilId",
                table: "Users",
                column: "ProfilId",
                principalTable: "Profils",
                principalColumn: "IdProfil",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationsId",
                table: "Imputations");

            migrationBuilder.DropForeignKey(
                name: "FK_Imputations_Users_UserId",
                table: "Imputations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Profils_ProfilId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProfilId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Imputations_StatusImputationsId",
                table: "Imputations");

            migrationBuilder.DropIndex(
                name: "IX_Imputations_UserId",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "ProfilId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StatusImputationsId",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Imputations");

            migrationBuilder.AddColumn<string>(
                name: "IdProfil",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilIdProfil",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdStatusImputations",
                table: "Imputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdUser",
                table: "Imputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusImputationsIdStatusImputations",
                table: "Imputations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdUser",
                table: "Imputations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdImputation",
                table: "DetailImputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfilIdProfil",
                table: "Users",
                column: "ProfilIdProfil");

            migrationBuilder.CreateIndex(
                name: "IX_Imputations_StatusImputationsIdStatusImputations",
                table: "Imputations",
                column: "StatusImputationsIdStatusImputations");

            migrationBuilder.CreateIndex(
                name: "IX_Imputations_UserIdUser",
                table: "Imputations",
                column: "UserIdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationsIdStatusImputations",
                table: "Imputations",
                column: "StatusImputationsIdStatusImputations",
                principalTable: "StatusImputations",
                principalColumn: "IdStatusImputations",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imputations_Users_UserIdUser",
                table: "Imputations",
                column: "UserIdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Profils_ProfilIdProfil",
                table: "Users",
                column: "ProfilIdProfil",
                principalTable: "Profils",
                principalColumn: "IdProfil",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
