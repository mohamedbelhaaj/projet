using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetBack.Migrations
{
    public partial class deletetypeclient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_TypeClients_TypeClientId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "TypeClients");

   

            migrationBuilder.AlterColumn<string>(
                name: "TypeClientId",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TypeClientId",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TypeClients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeClients", x => x.Id);
                });

         

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_TypeClients_TypeClientId",
                table: "Clients",
                column: "TypeClientId",
                principalTable: "TypeClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
