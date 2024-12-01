using Microsoft.EntityFrameworkCore.Migrations;

namespace HairSalon.Migrations
{
    public partial class DeleteFKEYfromNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewAppointments_AllAppointments_AllAppointmentId",
                table: "NewAppointments");

            migrationBuilder.DropIndex(
                name: "IX_NewAppointments_AllAppointmentId",
                table: "NewAppointments");

            migrationBuilder.DropColumn(
                name: "AllAppointmentId",
                table: "NewAppointments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllAppointmentId",
                table: "NewAppointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewAppointments_AllAppointmentId",
                table: "NewAppointments",
                column: "AllAppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewAppointments_AllAppointments_AllAppointmentId",
                table: "NewAppointments",
                column: "AllAppointmentId",
                principalTable: "AllAppointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
