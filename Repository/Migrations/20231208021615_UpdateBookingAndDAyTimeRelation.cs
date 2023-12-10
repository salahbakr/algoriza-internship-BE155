using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingAndDAyTimeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Time_Bookings_BookingId",
                table: "Time");

            migrationBuilder.DropIndex(
                name: "IX_Time_BookingId",
                table: "Time");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Time");

            migrationBuilder.AddColumn<int>(
                name: "TimeId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TimeId",
                table: "Bookings",
                column: "TimeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Time_TimeId",
                table: "Bookings",
                column: "TimeId",
                principalTable: "Time",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Time_TimeId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TimeId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TimeId",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Time",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Time_BookingId",
                table: "Time",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Time_Bookings_BookingId",
                table: "Time",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");
        }
    }
}
