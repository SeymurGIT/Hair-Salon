using System.ComponentModel.DataAnnotations;

namespace HairSalon.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu sahəni doldurun")]
        public string Name { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "Bu sahəni doldurun")]
        [EmailAddress(ErrorMessage = "Doğru olmayan email ünvanı")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu sahəni doldurun")]
        public string Message { get; set; }

        public string PhoneNumber { get; set; }
    }
}
