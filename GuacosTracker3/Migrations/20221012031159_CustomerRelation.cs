using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuacosTracker3.Migrations
{
    public partial class CustomerRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Notes_NotesId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "NotesId",
                table: "Notes");

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
                name: "FK_Ticket_Customers_CustomersId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Notes_NotesId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_CustomersId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "CustomersId",
                table: "Ticket");

            migrationBuilder.AddColumn<Guid>(
                name: "NotesId",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Notes_NotesId",
                table: "Notes",
                column: "NotesId",
                principalTable: "Notes",
                principalColumn: "Id");
        }
    }
}
