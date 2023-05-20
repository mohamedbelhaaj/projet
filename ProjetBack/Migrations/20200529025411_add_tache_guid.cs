using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ProjetBack.Migrations
{
    public partial class add_tache_guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_Taches", "Taches");
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Taches",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey("PK_Taches", "Taches", "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
