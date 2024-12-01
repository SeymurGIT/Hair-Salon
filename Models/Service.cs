using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HairSalon.Models
#nullable disable
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Bu sahə tələb olunur")]
        
        public string ServiceName { get; set; }
        
        [Required(ErrorMessage = "Bu sahə tələb olunur")]
        
        public string ServiceDescription { get; set; }
        
        [Required(ErrorMessage = "Bu sahə tələb olunur")]
        
        public byte ServicePrice {get; set;}
        
        public string ServiceImageLink { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

    }
}
