using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class deleteprojetPhase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projets_PhaseProjets_PhaseProjetId",
                table: "Projets");

            migrationBuilder.DropTable(
                name: "PhaseProjets");

            migrationBuilder.AlterColumn<string>(
                name: "PhaseProjetId",
                table: "Projets",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhaseProjetId",
                table: "Projets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PhaseProjets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseProjets", x => x.Id);
                });



            migrationBuilder.AddForeignKey(
                name: "FK_Projets_PhaseProjets_PhaseProjetId",
                table: "Projets",
                column: "PhaseProjetId",
                principalTable: "PhaseProjets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
