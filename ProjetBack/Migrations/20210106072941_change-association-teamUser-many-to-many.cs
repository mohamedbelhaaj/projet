using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class changeassociationteamUsermanytomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equips_Users_ManagerId",
                table: "Equips");

            migrationBuilder.DropIndex(
                name: "IX_Equips_ManagerId",
                table: "Equips");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Equips");

            migrationBuilder.CreateTable(
                name: "EquipeUser",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    EquipeId = table.Column<string>(nullable: false),
                    role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipeUser", x => new { x.UserId, x.EquipeId });
                    table.ForeignKey(
                        name: "FK_EquipeUser_Equips_EquipeId",
                        column: x => x.EquipeId,
                        principalTable: "Equips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipeUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipeUser_EquipeId",
                table: "EquipeUser",
                column: "EquipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipeUser");

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Equips",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equips_ManagerId",
                table: "Equips",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equips_Users_ManagerId",
                table: "Equips",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
