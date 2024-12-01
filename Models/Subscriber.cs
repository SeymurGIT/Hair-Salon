using System.ComponentModel.DataAnnotations;

namespace HairSalon.Models
{
    public class Subscriber
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Doğru email yazın")]
        
        public string EmailAddress { get; set; }
    }
}
