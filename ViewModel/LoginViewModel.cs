using System.ComponentModel.DataAnnotations;

namespace HairSalon.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Bu sahə boş ola bilməz ")]
        [EmailAddress(ErrorMessage = "Doğru olmayan email ünvanı")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Bu sahə boş ola bilməz ")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; } = true;

    }
}
