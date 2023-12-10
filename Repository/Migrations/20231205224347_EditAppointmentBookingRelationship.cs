using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class EditAppointmentBookingRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Time",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Time_BookingId",
                table: "Time",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentId",
                table: "Bookings",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Time_Bookings_BookingId",
                table: "Time",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Time_Bookings_BookingId",
                table: "Time");

            migrationBuilder.DropIndex(
                name: "IX_Time_BookingId",
                table: "Time");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Time");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentId",
                table: "Bookings",
                column: "AppointmentId",
                unique: true);
        }
    }
}
