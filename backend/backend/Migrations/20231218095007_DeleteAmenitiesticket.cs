using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAmenitiesticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesTickets_Amenities",
                table: "amenitiestickets");

            migrationBuilder.DropForeignKey(
                name: "FK_AmenitiesTickets_Tickets",
                table: "amenitiestickets");

            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "amenitiestickets");

            migrationBuilder.RenameTable(
                name: "amenitiestickets",
                newName: "Amenitiestickets");

            migrationBuilder.RenameColumn(
                name: "TicketID",
                table: "Amenitiestickets",
                newName: "TicketId");

            migrationBuilder.RenameColumn(
                name: "AmenityID",
                table: "Amenitiestickets",
                newName: "AmenityId");

            migrationBuilder.RenameIndex(
                name: "FK_AmenitiesTickets_Tickets",
                table: "Amenitiestickets",
                newName: "IX_Amenitiesticket_TicketId");

            migrationBuilder.AlterTable(
                name: "Amenitiestickets")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb3")
                .OldAnnotation("Relational:Collation", "utf8mb3_general_ci");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Amenitiestickets",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)",
                oldPrecision: 19,
                oldScale: 4);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Amenitiestickets",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amenitiesticket",
                table: "Amenitiestickets",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Amenitiesticket_AmenityId",
                table: "Amenitiestickets",
                column: "AmenityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Amenitiesticket_amenities_AmenityId",
                table: "Amenitiestickets",
                column: "AmenityId",
                principalTable: "amenities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Amenitiesticket_tickets_TicketId",
                table: "Amenitiestickets",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amenitiesticket_amenities_AmenityId",
                table: "Amenitiestickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Amenitiesticket_tickets_TicketId",
                table: "Amenitiestickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amenitiesticket",
                table: "Amenitiestickets");

            migrationBuilder.DropIndex(
                name: "IX_Amenitiesticket_AmenityId",
                table: "Amenitiestickets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Amenitiestickets");

            migrationBuilder.RenameTable(
                name: "Amenitiestickets",
                newName: "amenitiestickets");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "amenitiestickets",
                newName: "TicketID");

            migrationBuilder.RenameColumn(
                name: "AmenityId",
                table: "amenitiestickets",
                newName: "AmenityID");

            migrationBuilder.RenameIndex(
                name: "IX_Amenitiesticket_TicketId",
                table: "amenitiestickets",
                newName: "FK_AmenitiesTickets_Tickets");

            migrationBuilder.AlterTable(
                name: "amenitiestickets")
                .Annotation("MySql:CharSet", "utf8mb3")
                .Annotation("Relational:Collation", "utf8mb3_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "amenitiestickets",
                type: "decimal(19,4)",
                precision: 19,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddPrimaryKey(
                name: "PRIMARY",
                table: "amenitiestickets",
                columns: new[] { "AmenityID", "TicketID" })
                .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_AmenitiesTickets_Amenities",
                table: "amenitiestickets",
                column: "AmenityID",
                principalTable: "amenities",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AmenitiesTickets_Tickets",
                table: "amenitiestickets",
                column: "TicketID",
                principalTable: "tickets",
                principalColumn: "ID");
        }
    }
}
