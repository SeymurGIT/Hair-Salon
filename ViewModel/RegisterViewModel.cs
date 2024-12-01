using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace HairSalon.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        [StringLength(15)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifrə uyğun gəlmir!")]
        public string ConfirmPassword { get; set; } = string.Empty;


    }
}
