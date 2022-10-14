using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuacosTracker3.Migrations
{
    public partial class Test3Dv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Customers_CustomersId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_CustomersId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "CustomersId",
                table: "Ticket");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomersId",
                table: "Ticket",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CustomersId",
                table: "Ticket",
                column: "CustomersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Customers_CustomersId",
                table: "Ticket",
                column: "CustomersId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
