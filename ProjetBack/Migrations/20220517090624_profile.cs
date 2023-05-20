using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    profileId = table.Column<string>(nullable: false),
                    annee = table.Column<string>(nullable: true),
                    budget = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.profileId);
                });

            migrationBuilder.CreateTable(
                name: "profileUser",
                columns: table => new
                {
                    profileId = table.Column<string>(nullable: false),
                    userId = table.Column<string>(nullable: false),
                    dateDebut = table.Column<DateTime>(nullable: false),
                    dateFin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profileUser", x => new { x.userId, x.profileId });
                    table.ForeignKey(
                        name: "FK_profileUser_Profile_profileId",
                        column: x => x.profileId,
                        principalTable: "Profile",
                        principalColumn: "profileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_profileUser_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_profileUser_profileId",
                table: "profileUser",
                column: "profileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profileUser");

            migrationBuilder.DropTable(
                name: "Profile");
        }
    }
}
