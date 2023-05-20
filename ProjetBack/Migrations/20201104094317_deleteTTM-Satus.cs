using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class deleteTTMSatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetLivraisons_Status_StatusId",
                table: "ProjetLivraisons");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetLivraisons_TTM_TTMId",
                table: "ProjetLivraisons");



            migrationBuilder.AlterColumn<string>(
                name: "TTMId",
                table: "ProjetLivraisons",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatusId",
                table: "ProjetLivraisons",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TTMId",
                table: "ProjetLivraisons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatusId",
                table: "ProjetLivraisons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

  

  

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetLivraisons_Status_StatusId",
                table: "ProjetLivraisons",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "IdStatus",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetLivraisons_TTM_TTMId",
                table: "ProjetLivraisons",
                column: "TTMId",
                principalTable: "TTM",
                principalColumn: "IdTTM",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
