using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class alter_imputation_relation_imputationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "Unique_StatusImputations_Name",
                table: "StatusImputations",
                column: "Name");
            
            migrationBuilder.DropForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationIdStatusImputations",
                table: "Imputations");

            migrationBuilder.DropIndex(
                name: "IX_Imputations_StatusImputationIdStatusImputations",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "StatusImputationIdStatusImputations",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "StatusImputationsId",
                table: "Imputations");

            migrationBuilder.AddColumn<string>(
                name: "StatusImputationId",
                table: "Imputations",
                nullable: false,
                defaultValue: "1971333C-C472-4E75-9B41-632B2D38AB01");

            migrationBuilder.CreateIndex(
                name: "IX_Imputations_StatusImputationId",
                table: "Imputations",
                column: "StatusImputationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationId",
                table: "Imputations",
                column: "StatusImputationId",
                principalTable: "StatusImputations",
                principalColumn: "IdStatusImputations",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationId",
                table: "Imputations");

            migrationBuilder.DropIndex(
                name: "IX_Imputations_StatusImputationId",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "StatusImputationId",
                table: "Imputations");

            migrationBuilder.AddColumn<string>(
                name: "StatusImputationIdStatusImputations",
                table: "Imputations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusImputationsId",
                table: "Imputations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Imputations_StatusImputationIdStatusImputations",
                table: "Imputations",
                column: "StatusImputationIdStatusImputations");

            migrationBuilder.AddForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationIdStatusImputations",
                table: "Imputations",
                column: "StatusImputationIdStatusImputations",
                principalTable: "StatusImputations",
                principalColumn: "IdStatusImputations",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
