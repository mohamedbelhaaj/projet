using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class commercial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEdps_Commercants_CommercantId",
                table: "ProjetEdps");

            migrationBuilder.RenameColumn(
                name: "CommercantId",
                table: "ProjetEdps",
                newName: "Commercantid");

            migrationBuilder.RenameIndex(
                name: "IX_ProjetEdps_CommercantId",
                table: "ProjetEdps",
                newName: "IX_ProjetEdps_Commercantid");

            migrationBuilder.AddColumn<string>(
                name: "CommercialId",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetEdps_CommercialId",
                table: "ProjetEdps",
                column: "CommercialId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEdps_Commercants_Commercantid",
                table: "ProjetEdps",
                column: "Commercantid",
                principalTable: "Commercants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEdps_Users_CommercialId",
                table: "ProjetEdps",
                column: "CommercialId",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEdps_Commercants_Commercantid",
                table: "ProjetEdps");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEdps_Users_CommercialId",
                table: "ProjetEdps");

            migrationBuilder.DropIndex(
                name: "IX_ProjetEdps_CommercialId",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "CommercialId",
                table: "ProjetEdps");

            migrationBuilder.RenameColumn(
                name: "Commercantid",
                table: "ProjetEdps",
                newName: "CommercantId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjetEdps_Commercantid",
                table: "ProjetEdps",
                newName: "IX_ProjetEdps_CommercantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEdps_Commercants_CommercantId",
                table: "ProjetEdps",
                column: "CommercantId",
                principalTable: "Commercants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
