using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuacosTracker3.Migrations
{
    public partial class TickNoteCustRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Ticket_TicketId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_TicketId",
                table: "Notes");

            migrationBuilder.AddColumn<Guid>(
                name: "NotesId",
                table: "Ticket",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_NotesId",
                table: "Ticket",
                column: "NotesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Notes_NotesId",
                table: "Ticket",
                column: "NotesId",
                principalTable: "Notes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Notes_NotesId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_NotesId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "NotesId",
                table: "Ticket");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TicketId",
                table: "Notes",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Ticket_TicketId",
                table: "Notes",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
