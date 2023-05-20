using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class equipeuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          





   

            migrationBuilder.AddColumn<string>(
                name: "EquipeId",
                table: "Users",
                nullable: true);




            migrationBuilder.CreateTable(
                name: "Equips",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Nom = table.Column<string>(nullable: true),
                    ManagerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equips_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });




            migrationBuilder.CreateIndex(
                name: "IX_Equips_ManagerId",
                table: "Equips",
                column: "ManagerId");




            migrationBuilder.AddForeignKey(
                name: "FK_Users_Equips_EquipeId",
                table: "Users",
                column: "EquipeId",
                principalTable: "Equips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Equips_EquipeId",
                table: "Users");



            migrationBuilder.DropTable(
                name: "Equips");


            migrationBuilder.DropColumn(
                name: "EquipeId",
                table: "Users");





        
        }
    }
}
