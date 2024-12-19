using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuacosTracker3.Migrations
{
    public partial class RemoveNullabilityofRecentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RecentStatus",
                table: "Ticket",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RecentStatus",
                table: "Ticket",
                type: "nvarchar(25)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)");
        }
    }
}
