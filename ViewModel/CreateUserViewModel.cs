using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HairSalon.ViewModel
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifrə uyğun gəlmir!")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu sahə boş ola bilməz")]
        public string FullName { get; set; } = string.Empty;

        public IFormFile Image { get; set; }

        public string UserImageLink { get; set; } = string.Empty;
    }
}
