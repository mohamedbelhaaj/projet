using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ProjetBack.Migrations
{
    public partial class add_tache_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_Taches", "Taches");
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Taches",
                nullable: false,
                oldClrType: typeof(Guid),
                maxLength: 450,
                defaultValueSql: "NEWID()",
                oldNullable: false);

            migrationBuilder.AddPrimaryKey("PK_Taches", "Taches", "Id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
