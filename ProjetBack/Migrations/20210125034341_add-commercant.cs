using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class addcommercant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "CodeProjet",
            //    table: "ProjetEdps",
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommercantId",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Commercants",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    Nom = table.Column<string>(nullable: true),
                    Prenom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commercants", x => x.id);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProjetEdps_CodeProjet",
            //    table: "ProjetEdps",
            //    column: "CodeProjet",
            //    unique: true,
            //    filter: "[CodeProjet] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetEdps_CommercantId",
                table: "ProjetEdps",
                column: "CommercantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEdps_Commercants_CommercantId",
                table: "ProjetEdps",
                column: "CommercantId",
                principalTable: "Commercants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEdps_Commercants_CommercantId",
                table: "ProjetEdps");

            migrationBuilder.DropTable(
                name: "Commercants");

            migrationBuilder.DropIndex(
                name: "IX_ProjetEdps_CodeProjet",
                table: "ProjetEdps");

            migrationBuilder.DropIndex(
                name: "IX_ProjetEdps_CommercantId",
                table: "ProjetEdps");

            //migrationBuilder.DropColumn(
            //    name: "CodeProjet",
            //    table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "CommercantId",
                table: "ProjetEdps");
        }
    }
}
