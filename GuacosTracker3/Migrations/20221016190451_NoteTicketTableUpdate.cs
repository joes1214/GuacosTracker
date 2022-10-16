using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuacosTracker3.Migrations
{
    public partial class NoteTicketTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RecentChange",
                table: "Ticket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RecentStatus",
                table: "Ticket",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Notes",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecentChange",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "RecentStatus",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Notes");
        }
    }
}
