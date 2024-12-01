using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalon.Models
{
    
    public class AllAppointments
    {
        [Key]
        public int Id { get; set; }
        public int? NewAppointmentId { get; set; }
        [ForeignKey("NewAppointmentId")]
        public NewAppointment OriginalNewAppointment { get; set; }
    }
    }

