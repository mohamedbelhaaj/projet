using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class addidcodeProjet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEdps_CodeProjet_CodeProjetsId",
                table: "ProjetEdps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeProjet",
                table: "CodeProjet");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "CodeProjet",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "id",
                table: "CodeProjet",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeProjet",
                table: "CodeProjet",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEdps_CodeProjet_CodeProjetsId",
                table: "ProjetEdps",
                column: "CodeProjetsId",
                principalTable: "CodeProjet",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEdps_CodeProjet_CodeProjetsId",
                table: "ProjetEdps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CodeProjet",
                table: "CodeProjet");

            migrationBuilder.DropColumn(
                name: "id",
                table: "CodeProjet");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "CodeProjet",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CodeProjet",
                table: "CodeProjet",
                column: "Numero");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEdps_CodeProjet_CodeProjetsId",
                table: "ProjetEdps",
                column: "CodeProjetsId",
                principalTable: "CodeProjet",
                principalColumn: "Numero",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
