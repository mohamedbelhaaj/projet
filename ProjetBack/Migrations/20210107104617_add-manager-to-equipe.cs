using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class addmanagertoequipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Equips",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
