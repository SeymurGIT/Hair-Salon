using Microsoft.EntityFrameworkCore.Migrations;

namespace HairSalon.Migrations
{
    public partial class Revert20231124093911_FKeyFromAllToNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
