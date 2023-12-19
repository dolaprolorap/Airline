using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Schedule",
                table: "seats");

            migrationBuilder.DropIndex(
                name: "IX_seats_ScheduleId",
                table: "seats");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "seats",
                newName: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_seats_TicketId",
                table: "seats",
                column: "TicketId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Ticket",
                table: "seats",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Ticket",
                table: "seats");

            migrationBuilder.DropIndex(
                name: "IX_seats_TicketId",
                table: "seats");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "seats",
                newName: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_seats_ScheduleId",
                table: "seats",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Schedule",
                table: "seats",
                column: "ScheduleId",
                principalTable: "schedules",
                principalColumn: "ID");
        }
    }
}
