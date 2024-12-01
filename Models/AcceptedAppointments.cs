using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalon.Models
{
    public class AcceptedAppointments
    {
        [Key]
        public int AcceptedAppointmentId { get; set; }

        public int NewAppointmentId {get; set;}

        public DateTime AcceptedDate {get; set;}

        [ForeignKey("NewAppointmentId")]
        public NewAppointment OriginalAppointment {get;set;}
    }
}
