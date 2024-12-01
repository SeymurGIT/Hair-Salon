using System.ComponentModel.DataAnnotations;

namespace HairSalon.ViewModel
{
    public class ResetPasswordViewModel
    {

        [Required]
        public string Token { get; set; } = string.Empty;

        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifrə uyğun gəlmir.")]
        public string ConfirmPassword { get; set; } = string.Empty;


    }
}
