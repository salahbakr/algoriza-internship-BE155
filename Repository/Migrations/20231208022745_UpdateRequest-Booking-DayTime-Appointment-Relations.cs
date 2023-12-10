using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequestBookingDayTimeAppointmentRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Appointments_AppointmentId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_DoctorId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Time_Appointments_AppointmentId",
                table: "Time");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RequestId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Requests",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_DoctorId",
                table: "Requests",
                newName: "IX_Requests_ApplicationUserId");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "Time",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RequestId",
                table: "Bookings",
                column: "RequestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserId",
                table: "Requests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Time_Appointments_AppointmentId",
                table: "Time",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Time_Appointments_AppointmentId",
                table: "Time");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RequestId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Requests",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_ApplicationUserId",
                table: "Requests",
                newName: "IX_Requests_DoctorId");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "Time",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentId",
                table: "Bookings",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RequestId",
                table: "Bookings",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Appointments_AppointmentId",
                table: "Bookings",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_DoctorId",
                table: "Requests",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Time_Appointments_AppointmentId",
                table: "Time",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
