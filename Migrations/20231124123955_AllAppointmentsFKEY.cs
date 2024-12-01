using Microsoft.EntityFrameworkCore.Migrations;

namespace HairSalon.Migrations
{
    public partial class AllAppointmentsFKEY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AllAppointments_NewAppointmentId",
                table: "AllAppointments",
                column: "NewAppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllAppointments_NewAppointments_NewAppointmentId",
                table: "AllAppointments",
                column: "NewAppointmentId",
                principalTable: "NewAppointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllAppointments_NewAppointments_NewAppointmentId",
                table: "AllAppointments");

            migrationBuilder.DropIndex(
                name: "IX_AllAppointments_NewAppointmentId",
                table: "AllAppointments");
        }
    }
}
