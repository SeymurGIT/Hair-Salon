using System;

namespace HairSalon.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurName { get; set; }
        public DateTime AppointmentDate { get; set; }
        // Add other properties as needed

        public string Status { get; set; }
    }
}
