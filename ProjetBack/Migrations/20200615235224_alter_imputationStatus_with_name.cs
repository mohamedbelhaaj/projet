using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class alter_imputationStatus_with_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationsId",
                table: "Imputations");

            migrationBuilder.DropIndex(
                name: "IX_Imputations_StatusImputationsId",
                table: "Imputations");

           

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StatusImputations",
                maxLength: 250,
                nullable: false,
                defaultValue: "New");

            migrationBuilder.AlterColumn<string>(
                name: "StatusImputationsId",
                table: "Imputations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusImputationIdStatusImputations",
                table: "Imputations",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationIdStatusImputations",
                table: "Imputations");

            migrationBuilder.DropIndex(
                name: "IX_Imputations_StatusImputationIdStatusImputations",
                table: "Imputations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StatusImputations");

            migrationBuilder.DropColumn(
                name: "StatusImputationIdStatusImputations",
                table: "Imputations");

         

            migrationBuilder.AlterColumn<string>(
                name: "StatusImputationsId",
                table: "Imputations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Imputations_StatusImputationsId",
                table: "Imputations",
                column: "StatusImputationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imputations_StatusImputations_StatusImputationsId",
                table: "Imputations",
                column: "StatusImputationsId",
                principalTable: "StatusImputations",
                principalColumn: "IdStatusImputations",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
