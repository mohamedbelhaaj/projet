using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class clientProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailLivraisons_Clients_ClientId",
                table: "DetailLivraisons");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailLivraisons_Projets_ProjetId",
                table: "DetailLivraisons");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_Valideur1Id",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_Valideur2Id",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "TTM");

            migrationBuilder.DropIndex(
                name: "IX_Users_Valideur1Id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Valideur2Id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_DetailLivraisons_ClientId",
                table: "DetailLivraisons");

            migrationBuilder.DropColumn(
                name: "Valideur1Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Valideur2Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "DetailLivraisons");

            migrationBuilder.AlterColumn<string>(
                name: "ProjetId",
                table: "DetailLivraisons",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "ClientProjet",
                columns: table => new
                {
                    ProjetId = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProjet", x => new { x.ProjetId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_ClientProjet_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientProjet_Projets_ProjetId",
                        column: x => x.ProjetId,
                        principalTable: "Projets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProjet_ClientId",
                table: "ClientProjet",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailLivraisons_Projets_ProjetId",
                table: "DetailLivraisons",
                column: "ProjetId",
                principalTable: "Projets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailLivraisons_Projets_ProjetId",
                table: "DetailLivraisons");

            migrationBuilder.DropTable(
                name: "ClientProjet");

            migrationBuilder.AddColumn<string>(
                name: "Valideur1Id",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Valideur2Id",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjetId",
                table: "DetailLivraisons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "DetailLivraisons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    IdStatus = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.IdStatus);
                });

            migrationBuilder.CreateTable(
                name: "TTM",
                columns: table => new
                {
                    IdTTM = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTM", x => x.IdTTM);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Valideur1Id",
                table: "Users",
                column: "Valideur1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Valideur2Id",
                table: "Users",
                column: "Valideur2Id");

            migrationBuilder.CreateIndex(
                name: "IX_DetailLivraisons_ClientId",
                table: "DetailLivraisons",
                column: "ClientId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_Valideur1Id",
                table: "Users",
                column: "Valideur1Id",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_Valideur2Id",
                table: "Users",
                column: "Valideur2Id",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
