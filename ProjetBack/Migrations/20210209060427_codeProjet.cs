using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class codeProjet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeProjetsId",
                table: "ProjetEdps",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CodeProjet",
                columns: table => new
                {
                    Numero = table.Column<string>(nullable: false),
                    Nature = table.Column<string>(nullable: true),
                    client = table.Column<string>(nullable: true),
                    Intitule = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeProjet", x => x.Numero);
                    table.ForeignKey(
                        name: "FK_CodeProjet_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjetEdps_CodeProjetsId",
                table: "ProjetEdps",
                column: "CodeProjetsId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeProjet_ClientId",
                table: "CodeProjet",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetEdps_CodeProjet_CodeProjetsId",
                table: "ProjetEdps",
                column: "CodeProjetsId",
                principalTable: "CodeProjet",
                principalColumn: "Numero",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetEdps_CodeProjet_CodeProjetsId",
                table: "ProjetEdps");

            migrationBuilder.DropTable(
                name: "CodeProjet");

            migrationBuilder.DropIndex(
                name: "IX_ProjetEdps_CodeProjetsId",
                table: "ProjetEdps");

            migrationBuilder.DropColumn(
                name: "CodeProjetsId",
                table: "ProjetEdps");
        }
    }
}
