using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalon.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public int NewAppointmentId { get; set; }

        public DateTime FirstAppliedDate { get; set; }

        [ForeignKey("NewAppointmentId")]
        public NewAppointment TheCustomerAppointment { get; set; }

    }
}
