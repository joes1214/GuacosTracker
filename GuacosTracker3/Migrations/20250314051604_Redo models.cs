using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuacosTracker3.Migrations
{
    public partial class Redomodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "RecentStatus",
                table: "Ticket");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Ticket",
                newName: "EmployeeID");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Ticket",
                newName: "CurrentStatus");

            migrationBuilder.RenameColumn(
                name: "Hidden",
                table: "Ticket",
                newName: "IsClosed");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Ticket",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "Customer",
                table: "Ticket",
                newName: "CustomerID");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "Notes",
                newName: "TicketID");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Notes",
                newName: "EmployeeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Ticket",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "IsClosed",
                table: "Ticket",
                newName: "Hidden");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Ticket",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Ticket",
                newName: "Customer");

            migrationBuilder.RenameColumn(
                name: "CurrentStatus",
                table: "Ticket",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "TicketID",
                table: "Notes",
                newName: "TicketId");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Notes",
                newName: "EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Ticket",
                type: "nvarchar(MAX)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecentStatus",
                table: "Ticket",
                type: "nvarchar(25)",
                nullable: true);
        }
    }
}
