using Castle.MicroKernel.SubSystems.Conversion;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalon.Models
{
    
    public class NewAppointment
    {
        public NewAppointment()
        {
            AppointmentDate = DateTime.Today;
            AppointmentTime = new TimeSpan(9, 0, 0);
        }
        [Key]
        public int Id { get; set; }
       

        public string QueueNumber { get; set; }
        [Required]
        public string CustomerName { get; set; }
        
        [Required]
        public string CustomerSurname { get; set; }
        
        public string CustomerEmail { get; set; }
        
        [Required]
        public string CustomerPhoneNumber { get; set; }
        
        [Required]
        [Column(TypeName="DATE")]
        public DateTime AppointmentDate { get; set; }
        
        public TimeSpan AppointmentTime { get; set; }
        
        [Required]
        public string SelectedServices {get; set;}
        
        public string AppointmentImageLink {get; set;}
        
        public DateTime ApplyDate { get; set; }

        [NotMapped]
        public IFormFile HairStyle { get; set; }

        
        public string Status { get; set; }

        


    }
}
