using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAmenitiesTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "amenitiestickets");

            migrationBuilder.RenameIndex(
                name: "FK_AmenitiesTickets_Tickets",
                table: "amenitiestickets",
                newName: "IX_amenitiestickets_TicketID");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "amenitiestickets",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PRIMARY",
                table: "amenitiestickets",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_amenitiestickets_AmenityID",
                table: "amenitiestickets",
                column: "AmenityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "amenitiestickets");

            migrationBuilder.DropIndex(
                name: "IX_amenitiestickets_AmenityID",
                table: "amenitiestickets");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "amenitiestickets");

            migrationBuilder.RenameIndex(
                name: "IX_amenitiestickets_TicketID",
                table: "amenitiestickets",
                newName: "FK_AmenitiesTickets_Tickets");

            migrationBuilder.AddPrimaryKey(
                name: "PRIMARY",
                table: "amenitiestickets",
                columns: new[] { "AmenityID", "TicketID" })
                .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
        }
    }
}
