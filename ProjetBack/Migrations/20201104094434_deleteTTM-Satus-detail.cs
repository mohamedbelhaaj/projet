using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class deleteTTMSatusdetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailLivraisons_Status_StatusId",
                table: "DetailLivraisons");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailLivraisons_TTM_TTMId",
                table: "DetailLivraisons");



            migrationBuilder.AlterColumn<string>(
                name: "TTMId",
                table: "DetailLivraisons",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatusId",
                table: "DetailLivraisons",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TTMId",
                table: "DetailLivraisons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatusId",
                table: "DetailLivraisons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

   


            migrationBuilder.AddForeignKey(
                name: "FK_DetailLivraisons_Status_StatusId",
                table: "DetailLivraisons",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "IdStatus",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailLivraisons_TTM_TTMId",
                table: "DetailLivraisons",
                column: "TTMId",
                principalTable: "TTM",
                principalColumn: "IdTTM",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
