using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuacosTracker3.Migrations
{
    public partial class AddHiddentoTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "Ticket",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "Ticket");
        }
    }
}
