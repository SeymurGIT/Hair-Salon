using Microsoft.EntityFrameworkCore.Migrations;

namespace HairSalon.Migrations
{
    public partial class InitialAllAppointments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_AllAppointments_AcceptedAppointments_AcceptedAppointmentId",
                table: "AllAppointments");

            migrationBuilder.DropIndex(
                name: "IX_AllAppointments_AcceptedAppointmentId",
                table: "AllAppointments");

            migrationBuilder.DropColumn(
                name: "AcceptedAppointmentId",
                table: "AllAppointments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcceptedAppointmentId",
                table: "AllAppointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AllAppointments_AcceptedAppointmentId",
                table: "AllAppointments",
                column: "AcceptedAppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllAppointments_AcceptedAppointments_AcceptedAppointmentId",
                table: "AllAppointments",
                column: "AcceptedAppointmentId",
                principalTable: "AcceptedAppointments",
                principalColumn: "AcceptedAppointmentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
